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
//            foreach (var s in RSSLib.RSSDatabase.Test("Demo"))
            {
//                Console.WriteLine(s);
            }

            var sets = new List<Setting>
            {
                new Setting() {
                    blogName = "はちま起稿",
                    url = "http://blog.esuteru.com/index.rdf",
                    interval = 150,
                    item = "item",
                    title = "title",
                    link = "link",
                    description = "description",
                    blogTitle = "title",
                },
                new Setting() {
                    blogName = "かれっじライフハッキング",
                    url = "http://college2ch.blomaga.jp/index.rdf",
                    interval = 30,
                    item = "item",
                    title = "title",
                    link = "link",
                    description = "description",
                    blogTitle = "title",
                },
                new Setting() {
                    blogName = "VIPPER速報",
                    url = "http://vippers.jp/index.rdf",
                    interval = 15,
                    item = "item",
                    title = "title",
                    link = "link",
                    description = "description",
                    blogTitle = "title",
                }
            };
            Console.WriteLine(RSSMngr.ResetTimer(sets).Interval);

            Setting();
            Console.ReadKey();
        }

        static List<RSSInfo> GetList()
        {
            string page = "http://college2ch.blomaga.jp/index.rdf";
            //string page = "http://blog.esuteru.com/index.rdf";
            var list = RSSProc.GetRSSInfo(page, new string[] {"item", "title", "link", "description", "title"});
            foreach (var i in list)
            {
                Console.WriteLine(i.description);
            }
            return list;
        }

        static void Setting()
        {
            var sets = new List<Setting>
            {
                new Setting() {
                    blogName = "はちま起稿",
                    url = "http://blog.esuteru.com/index.rdf",
                    interval = 150,
                    item = "item",
                    title = "title",
                    link = "link",
                    description = "description",
                    blogTitle = "title",
                },
                new Setting() {
                    blogName = "かれっじライフハッキング",
                    url = "http://college2ch.blomaga.jp/index.rdf",
                    interval = 30,
                    item = "item",
                    title = "title",
                    link = "link",
                    description = "description",
                    blogTitle = "title",
                },
                new Setting() {
                    blogName = "VIPPER速報",
                    url = "http://vippers.jp/index.rdf",
                    interval = 15,
                    item = "item",
                    title = "title",
                    link = "link",
                    description = "description",
                    blogTitle = "title",
                }
            };

            InitXML.SaveXML(sets);
            var setting = InitXML.LoadXML();
            foreach (var s in setting)
            {
                Console.WriteLine(s.blogName);
            }
        }
    }
}
