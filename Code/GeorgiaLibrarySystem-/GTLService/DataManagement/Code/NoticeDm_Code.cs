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
        private static Timer _timer;
        
        public NoticeDm_Code(LendingDa_Code lendingDa, MemberDa_Code memberDa)
        {
            _lendingDa = lendingDa;
            _memberDa = memberDa;
            
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
            NoticeFilling();

            DateTime now = DateTime.Now;
            DateTime tomorrow = now.AddDays(1).Date;
            _timer.Interval = (tomorrow - now).TotalMilliseconds;//next interval at midnight
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
}