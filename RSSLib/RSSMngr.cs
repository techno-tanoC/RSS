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
        private class Unit
        {
            public Setting set;
            public RSSInfo lastInfo; //nullの可能性あり
        }

        private const int MINITES = 60000;

        private List<Unit> units;
        private Timer timer;

        public Action<object, TimerEventArgs> AfterGetInfomation;

        public RSSMngr()
        {
            timer = new Timer();
            timer.Interval = 10 * 1000;
            timer.Elapsed += TimerElapsed;
            Reload();
//            StartTimer();
            units.Select(x => x.lastInfo).ToList().ForEach(x => Console.WriteLine(x == null ? "" : x.title));
        }

        public void StartTimer()
        {
            timer.Start();
        }
        public void StopTimer()
        {
            timer.Stop();
        }

        public void Reload()
        {
            units = InitXML.LoadXML().Select(x =>
                new Unit()
                {
                    set = x,
                    lastInfo = RSSDatabase.GetLastInfo(x.tableName),
                }).ToList();
            units.ForEach(x =>
            {
                try
                {
                    RSSDatabase.Create(x.set.tableName);
                }
                catch { }
            });
        }

        public void SaveSetting()
        {
            InitXML.SaveXML(units.Select(x => x.set).ToList());
        }
        
        private static int GetCommonDivisor(int x, int y)
        {
            if (y == 0) return x;
            else
                return GetCommonDivisor(y, x % y);
        }

        private List<RSSInfo> InsertDB(Unit unit)
        {
            var list = new List<RSSInfo>();
            if (unit.lastInfo != null)
                RSSDatabase.Insert(unit.set.tableName, list = RSSProc.GetDiffInfo(unit.set, unit.lastInfo.link));
            else
                RSSDatabase.Insert(unit.set.tableName, list = RSSProc.GetRSSInfo(unit.set.url, unit.set.tableName));
            return list;
        }

        private int count = 0;
        private void TimerElapsed(object sender, EventArgs e)
        {
            var list = units
//                .Where(x => x.set.interval % timer.Interval * count < 10)
                .Aggregate(new List<RSSInfo>(), (sum, x) => {
                    sum.AddRange(InsertDB(x));
                    return sum;
                }).ToList();

            if (AfterGetInfomation != null)
                AfterGetInfomation(timer, new TimerEventArgs(list));
            count++;
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
