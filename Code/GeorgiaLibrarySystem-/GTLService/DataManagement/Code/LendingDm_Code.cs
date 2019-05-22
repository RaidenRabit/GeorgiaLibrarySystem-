using System;
using System.Timers;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LendingDm_Code: ILendingDm
    {
        private readonly LendingDa_Code _lendingDa;
        private readonly MemberDa_Code _memberDa;
        
        public LendingDm_Code(LendingDa_Code lendingDa, MemberDa_Code memberDa)
        {
            _lendingDa = lendingDa;
            _memberDa = memberDa;
        }

        public bool LendBook(int ssn, int copyId)
        {
            try
            {
                if (_lendingDa.MemberBorrowedBooks(ssn) < _memberDa.GetMember(ssn).MemberType.NrOfBooks //member is allowed to borrow more
                        && _lendingDa.GetBorrow(copyId) == null)//not borrowed at the time
                {
                    return _lendingDa.LendBook(new Borrow {SSN = ssn, CopyID = copyId, FromDate = DateTime.Now});
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool ReturnBook(int copyId)
        {
            try
            {
                Borrow borrow = _lendingDa.GetBorrow(copyId);
                if (borrow != null)
                {
                    borrow.ToDate = DateTime.Now;
                    return _lendingDa.SaveBorrowChanges();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool NoticeFilling()
        {
            foreach (var borrow in _lendingDa.GetAllActiveBorrows())
            {
                Member member = _memberDa.GetMember(borrow.SSN);
                
                if (DateTime.Now >= borrow.FromDate.AddDays(member.MemberType.LendingLenght + member.MemberType.GracePeriod) && borrow.noticeSent == null)
                {
                    borrow.noticeSent = false;
                }
            }
            return _lendingDa.SaveBorrowChanges();
        }
    }

    public class NoticeTimer
    {
        private static Timer _timer;

        public NoticeTimer()
        {
            _timer = new Timer
            {
                Enabled = true
            };
            OnTimedEvent(null, null);
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Context context = new Context();
            new LendingDm_Code(new LendingDa_Code(context), new MemberDa_Code(context)).NoticeFilling();

            DateTime now = DateTime.Now;
            DateTime tomorrow = now.AddDays(1).Date;
            _timer.Interval = (tomorrow - now).TotalMilliseconds;//next interval at midnight
        }
    }
}