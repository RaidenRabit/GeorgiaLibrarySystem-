using System;
using System.Timers;
using Core;
using GTLService.DataAccess.Code;

namespace GTLService.DataManagement.Code
{
    public class TimerDM_Code
    {
        private static Timer _timer;

        public TimerDM_Code()
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