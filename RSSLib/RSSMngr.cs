using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;

namespace RSSLib
{
    public class RSSMngr
    {
        private const int MINITES = 60000;

        public List<Setting> setting;
        public List<RSSInfo> infos;
        public Timer timer;
        public Action<object, TimerEventArgs> BeforeGetInfomation;
        public Action<object, TimerEventArgs> AfterGetInfomation;

        public RSSMngr()
        {
            timer = new Timer();
            setting = InitXML.LoadXML();
            
        }

        public Timer ResetTimer(List<Setting> setting)
        {
            timer.Stop();

            this.setting = InitXML.LoadXML();
            timer.Interval = this.setting.Select(x => x.interval)
                .Aggregate(0, (ans, each) => ans = GetCommonDivisor(ans, each));
            timer.Elapsed += TimerEvent;

            timer.Start();
            return timer;
        }
        private static int GetCommonDivisor(int x, int y)
        {
            if (y == 0) return x;
            else
                return GetCommonDivisor(y, x % y);
        }

        private int count = 0;
        private void TimerEvent(object sender, ElapsedEventArgs e)
        {
            if (BeforeGetInfomation != null)
            {
                var args = new TimerEventArgs(setting, infos);
                BeforeGetInfomation(sender, args);
            }

            // TODO
            foreach (var set in setting)
            {
                if (set.interval == timer.Interval * count)
                    ; //
            }

            if (AfterGetInfomation != null)
            {
                var args = new TimerEventArgs(setting, infos);
                AfterGetInfomation(sender, args);
            }

        }
    }

    public class TimerEventArgs : EventArgs
    {
        public List<Setting> setting { get; set; }
        public List<RSSInfo> infos { get; set; }
        public TimerEventArgs(List<Setting> setting, List<RSSInfo> infos)
        {
            this.setting = setting;
            this.infos = infos;
        }
    }
}
