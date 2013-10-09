using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;

namespace RSSLib
{
    public class Setting
    {
        public string blogName;
        public string url;
        public int interval;
        public string item;
        public string title;
        public string link;
        public string description;
        public string blogTitle;
    }

    public class InitXML
    {
        static InitXML()
        {
        }

        /// <summary>
        /// setting.xmlに書き込む
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static List<Setting> SaveXML(List<Setting> setting)
        {
            try
            {
                using (var fs = new FileStream("setting.xml", FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(List<Setting>));
                    serializer.Serialize(fs, setting);
                }
            }
            catch //setting.xmlのオープンに失敗した時
            {
                throw new RSSException("設定ファイルのオープンに失敗しました");
            }

            return setting;
        }

        /// <summary>
        /// setting.xmlを読み込んで返す
        /// </summary>
        /// <returns>setting.xmlがなければnull あれば中身</returns>
        public static List<Setting> LoadXML()
        {
            List<Setting> setting = null;
            try
            {
                using (var fs = new FileStream("setting.xml", FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(List<Setting>));
                    setting = (List<Setting>)serializer.Deserialize(fs);
                }
            }
            catch //setting.xmlがなかった時
            { }

            return setting;
        }
    }
}
