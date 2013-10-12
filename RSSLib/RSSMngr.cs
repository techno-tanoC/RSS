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

        public Action<object, TimerEventArgs> AfterGetInfomation;

        public RSSMngr()
        {
        }

        private static int GetCommonDivisor(int x, int y)
        {
            if (y == 0) return x;
            else
                return GetCommonDivisor(y, x % y);
        }
    }

    public class TimerEventArgs : EventArgs
    {

        public List<RSSInfo> infos { get; set; }
        public TimerEventArgs(List<RSSInfo> infos)
        {
            this.infos = infos;
        }
    }
}
