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

        public RSSMngr()
        {

        }

        public Timer ResetTimer(List<Setting> setting)
        {
            timer.Stop();

            timer.Interval = setting.Select(x => x.interval)
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

        private void TimerEvent(object sender, ElapsedEventArgs e)
        {
            if (timerHandler != null)
            {
                var args = new TimerEventArgs(setting, infos);
                timerHandler(sender, args);
            }

        }
    }

    public class TimerEventArgs : EventArgs
    {
        public List<Setting> setting;
        public List<RSSInfo> infos;
        public TimerEventArgs(List<Setting> setting, List<RSSInfo> infos)
        {
            this.setting = setting;
            this.infos = infos;
        }
    }
}
