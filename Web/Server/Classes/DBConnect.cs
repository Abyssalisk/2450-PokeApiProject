using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Server.Classes
{
    public class DBConnect
    {
        public MySqlConnection MyConnection { get; set; }
        public DBConnect()
        {
            using (MySqlConnection connect = new MySqlConnection())
            {
                try
                {
                    connect.ConnectionString = GetRDSConnectionString();
                    connect.Open();
                }
                catch { }
                finally
                {
                    if (connect != null || connect.State == System.Data.ConnectionState.Open)
                        MyConnection = connect;
                }
            }
        }

        public static string GetRDSConnectionString()
        {
            var dbname = "sql3346222";

            if (string.IsNullOrEmpty(dbname)) return null;

            string username = "sql3346222";
            string password = "wTvU3pVa7f";
            string hostname = "sql3.freemysqlhosting.net";
            string port = "3306";
            var connectionString = "Server=" + hostname + "; Port=" + port + "; Database=" + dbname + "; Uid=" + username + "; Pwd=" + password + ";";
            return connectionString;
        }
    }
}
