using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RSSLib;

namespace RSSTest
{
    class Program
    {
        static void Main(string[] args)
        {
//            RSSDatabase.Create("Demo");
//            RSSDatabase.Insert("Demo", GetList());
//            foreach (var s in RSSDatabase.Test("Demo"))
//                Console.WriteLine(s);
//            GetList();
//            Console.WriteLine(RSSDatabase.GetLastInfo("Demo").get());
            var mngr = new RSSMngr();
            mngr.AfterGetInfomation += handler;
            Console.ReadLine();
        }

        private static void handler(object sender, TimerEventArgs e)
        {
            e.infos.ForEach(x => Console.WriteLine(x.title));
        }

        static List<RSSInfo> GetList()
        {
            string page = "http://college2ch.blomaga.jp/index.rdf";
            //string page = "http://blog.esuteru.com/index.rdf";
            var list = RSSProc.GetRSSInfo(page, new string[] {"item", "title", "link", "description", "link"}, "college");
            foreach (var i in list)
            {
                Console.WriteLine(i.blogUrl);
            }
            return list;
        }

        static List<Setting> Setting()
        {
            var sets = new List<Setting>
            {
                new Setting() {
                    blogName = "はちま起稿",
                    url = "http://blog.esuteru.com/index.rdf",
                    tableName = "hatima",
                    interval = 5,
                    sets = new string[]
                    {
                        "item",
                        "title",
                        "link",
                        "description",
                        "link",
                    },
                },
                new Setting() {
                    blogName = "かれっじライフハッキング",
                    url = "http://college2ch.blomaga.jp/index.rdf",
                    tableName = "college",
                    interval = 5,
                    sets = new string[]
                    {
                        "item",
                        "title",
                        "link",
                        "description",
                        "link",
                    },
                },
                new Setting() {
                    blogName = "VIPPER速報",
                    url = "http://vippers.jp/index.rdf",
                    tableName = "vipper",
                    interval = 5,
                    sets = new string[]
                    {
                        "item",
                        "title",
                        "link",
                        "description",
                        "link",
                    },
                }
            };
            
            InitXML.SaveXML(sets);
            var setting = InitXML.LoadXML();
            foreach (var s in setting)
            {
                Console.WriteLine(s.blogName);
            }
            return setting;
        }
    }
}
