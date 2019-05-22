using System;
using System.Timers;
using Core;
using GTLService.DataAccess.Code;

namespace GTLService.DataManagement.Code
{
    public class NoticeDm_Code
    {
        private readonly LendingDa_Code _lendingDa;
        private readonly MemberDa_Code _memberDa;
        private readonly NoticeDa_Code _noticeDa;
        private static Timer _timer;
        
        public NoticeDm_Code(LendingDa_Code lendingDa, MemberDa_Code memberDa, NoticeDa_Code noticeDa)
        {
            _lendingDa = lendingDa;
            _memberDa = memberDa;
            _noticeDa = noticeDa;
            
            _timer = new Timer
            {
                Enabled = true
            };
            OnTimedEvent(null, null);
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            foreach (var borrow in _lendingDa.GetAllActiveBorrows())
            {
                Member member = _memberDa.GetMember(borrow.SSN);
                
                if (DateTime.Now >= borrow.FromDate.AddDays(member.MemberType.LendingLenght + member.MemberType.GracePeriod) && _noticeDa.GetNotice(borrow) == null)
                {
                    Notice notice = new Notice
                    {
                        SSN = borrow.SSN, 
                        CopyID = borrow.CopyID, 
                        FromDate = borrow.FromDate,
                        noticeSent = false
                    };
                    _noticeDa.CreateNotice(notice);
                }
            }

            DateTime now = DateTime.Now;
            DateTime tomorrow = now.AddDays(1).Date;

            _timer.Interval = (tomorrow - now).TotalMilliseconds;
        }
    }
}