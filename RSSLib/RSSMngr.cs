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

        static List<Setting> setting;
        static Timer timer;
        static RSSMngr()
        {
            timer = new Timer();
        }

        public RSSMngr()
        {

        }

        public static Timer ResetTimer(List<Setting> setting)
        {
            timer.Interval = setting.Select(x => x.interval)
                .Aggregate(0, (ans, each) => ans = GetCommonDivisor(ans, each));
            return timer;
        }
        private static int GetCommonDivisor(int x, int y)
        {
            if (y == 0) return x;
            else
                return GetCommonDivisor(y, x % y);
        }

    }
}
