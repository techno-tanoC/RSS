using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;

namespace RSSLib
{
    /// <summary>
    /// RSSの情報
    /// </summary>
    public class RSSInfo
    {
        public RSSInfo() { }
        public RSSInfo(string title, string link, string description, string imgurl, string blogUrl, string tableName)
        {
            this.title = title;
            this.link = link;
            this.description = description;
            this.imageUrl = imgurl;
            this.blogUrl = blogUrl;
            this.tableName = tableName;
        }

        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
        public string blogUrl { get; set; }
        public string tableName { get; set; }
        public string get()
        {
            return title + ":" + link + ":" + (imageUrl != null ? imageUrl : "") + ":" + blogUrl;
        }
    }

    /// <summary>
    /// RSS処理
    /// </summary>
    public class RSSProc
    {
        private const string anchor = "<img [^>]*?src=\"(?<img>[^>]*?(?:jpg|png|gif))\"";
        private static Regex re = new Regex(anchor, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static XNamespace ns = "http://purl.org/rss/1.0/";

        static RSSProc()
        {

        }

        /// <summary>
        /// RSSパース
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>List<RSSInfo></returns>
        public static List<RSSInfo> GetRSSInfo(string url, string tableName)
        {
            return GetRSSInfo(url, new Dictionary<string, string>()
            {
                { "item", "item" },
                { "title", "title" },
                { "link", "link" },
                { "description", "description" },
                { "blogUrl", "link" },
            }, tableName);
        }

        public static List<RSSInfo> GetRSSInfo(string url, string[] opts, string tableName)
        {
            if (opts.Length != 5)
                throw new RSSException("オプションの数が正しくありません");
            try
            {
                return GetRSSInfo(url, new Dictionary<string, string>()
                {
                    { "item", opts[0] },
                    { "title", opts[1] },
                    { "link", opts[2] },
                    { "description", opts[3] },
                    { "blogUrl", opts[4] },
                }, tableName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<RSSInfo> GetRSSInfo(string url, List<string> opts, string tableName)
        {
            if (opts.Count != 5)
                throw new RSSException("オプションの数が正しくありません");
            try
            {
                return GetRSSInfo(url, new Dictionary<string, string>()
                {
                    { "item", opts[0] },
                    { "title", opts[1] },
                    { "link", opts[2] },
                    { "description", opts[3] },
                    { "blogUrl", opts[4] },
                }, tableName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<RSSInfo> GetRSSInfo(string url, Setting setting, string tableName)
        {
            if (setting.sets[0] != "" && setting.sets[1] != "" && setting.sets[2] != "" && setting.sets[3] != "" && setting.sets[4] != "")
            {
                throw new RSSException("正しくない設定があります");
            }

            try
            {
                return GetRSSInfo(url, new Dictionary<string, string>()
                {
                    { "item", setting.sets[0] },
                    { "title", setting.sets[1] },
                    { "link", setting.sets[2] },
                    { "description", setting.sets[3] },
                    { "blogUrl", setting.sets[4] },
                }, tableName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// URLとオプションでXMLパース
        /// </summary>
        /// <param name="url">XMLのURL</param>
        /// <param name="opts">item title link description blogTitle</param>
        /// <returns>List<RSSInfo></returns>
        public static List<RSSInfo> GetRSSInfo(string url, Dictionary<string, string> opts, string tableName)
        {
            if (opts.Count != 5)
                throw new RSSException("オプションの数が正しくありません");
            if (! opts.All(x => x.Value != "" && x.Value != null))
                throw new RSSException("正しくないオプションがあります");

            XDocument xml = null;
            try
            {
                xml = XDocument.Load(url);
            }
            catch
            {
                throw new RSSException("XMLのロードに失敗しました");
            }

            var list = new List<RSSInfo>();
            try
            {
                list = xml.Descendants(ns + opts["item"])
                    .Select(x => new RSSInfo()
                    {
                        title = x.Descendants(ns + opts["title"]).First().Value,
                        link = x.Descendants(ns + opts["link"]).First().Value,
                        description = x.Descendants(ns + opts["description"]).First().Value,
                        imageUrl = re.Match(x.ToString()).Groups["img"].Value,
                        blogUrl = xml.Descendants(ns + opts["blogUrl"]).First().Value,
                        tableName = tableName,
                    }
                    ).ToList();
            }
            catch
            {
                throw new RSSException("XMLのパースに失敗しました オプションが間違っている可能性があります");
            }

            return list;
        }

        public static XDocument GetXML(string url)
        {
            return XDocument.Load(url);
        }

        public static List<RSSInfo> GetDiffInfo(Setting set, string recent, string tableName)
        {
            try
            {
                return GetRSSInfo(set.url, set, tableName)
                    .TakeWhile(x => x.link != recent).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class RSSException : Exception
    {
        public RSSException(string message)
            : base(message)
        {

        }
    }
}
