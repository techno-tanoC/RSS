using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.Data.SQLite.Generic;

namespace RSSLib
{
    public class RSSDatabase
    {
        const string file = "RSS.db";
        const string seq = "sqlite_sequence";
        const string master = "sqlite_master";

        public static List<string> Test(string demo)
        {
            using (var conn = new SQLiteConnection("Data Source=" + file)) //データベースに接続
            using (var comm = new SQLiteCommand()) //コマンドクラス
            {
                var ret = new List<string>();

                conn.Open();
                comm.Connection = conn; //接続するデータベースを設定
                var transaction = conn.BeginTransaction();
                
                /*
                comm.CommandText = "drop table Demo";
                comm.ExecuteNonQuery();
                */

                comm.CommandText = "select * from " + demo;
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(reader[2].ToString());
                    }
                }
                
                transaction.Commit();
                transaction.Dispose();

                return ret;
            }
        }

        public static List<string> GetMaster()
        {
            var ret = new List<string>();
            using (var conn = new SQLiteConnection("Data Source=" + file)) //データベースに接続
            using (var comm = new SQLiteCommand()) //コマンドクラス
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandText = "select * from " + master;
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(reader[1].ToString());
                    }
                }
            }
            return ret;
        }
        public static List<string> GetSequence()
        {
            var ret = new List<string>();
            using (var conn = new SQLiteConnection("Data Source=" + file)) //データベースに接続
            using (var comm = new SQLiteCommand()) //コマンドクラス
            {
                comm.Connection = conn;
                comm.CommandText = "select * form " + seq;
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(reader[0].ToString());
                    }
                }
            }
            return ret;
        }

        public static void Create(string tableName)
        {
            using (var conn = new SQLiteConnection("Data Source=" + file))
            using (var comm = new SQLiteCommand(conn))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    ExecuteNonQuery(comm, "create table " + tableName + "(id integer not null primary key autoincrement, Title text, Link text, Description text, ImageUrl text, BlogName text)");
                    transaction.Commit();
                }
            }
        }
        
        public static int Insert(string tableName, List<RSSInfo> infos)
        {
            int ret = 0;
            string sql = "insert into " + tableName + "(Title,Link,Description,ImageUrl,BlogName) values (@Title,@Link,@Description,@ImageUrl,@BlogName)";
            using (var conn = new SQLiteConnection("Data Source=" + file))
            using (var comm = new SQLiteCommand(conn))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var info in infos)
                    {
                        ExecuteNonQuery(comm, sql,
                            new SQLiteParameter[]
                        {
                            new SQLiteParameter("@Title", info.title),
                            new SQLiteParameter("@Link", info.link),
                            new SQLiteParameter("@Description", info.description),
                            new SQLiteParameter("@ImageUrl", info.imageUrl),
                            new SQLiteParameter("@BlogName", info.blogTitle),
                        });
                    }
                    transaction.Commit();
                }
            }
            return ret;
        }

        /// <summary>
        /// トランザクションする用
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SQLiteCommand comm, string sql)
        {
            return ExecuteNonQuery(comm, sql, null);
        }
        public static int ExecuteNonQuery(SQLiteCommand comm, string sql, SQLiteParameter[] param)
        {
            try
            {
                comm.CommandText = sql;
                if (param != null)
                    comm.Parameters.AddRange(param);
                return comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string GetLastLink(string tableName)
        {
            string sql = "select max(id), Link from " + tableName;
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + file))
                using (var comm = conn.CreateCommand())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        using (var reader = ExecuteQuery(comm, sql))
                        {
                            if (reader.Read())
                                return reader[1].ToString();
                            else
                                throw new RSSException("データベースの取得に失敗しました");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static SQLiteDataReader ExecuteQuery(SQLiteCommand comm, string sql)
        {
            try
            {
                comm.CommandText = sql;
                return comm.ExecuteReader();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
