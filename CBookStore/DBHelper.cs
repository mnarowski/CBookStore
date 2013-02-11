using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBookStore
{
    class DBHelper
    {
        private static System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(
                                       "User Id=st146732;" +
                                       "password=p146732;"+
                                       "Server=148.81.130.59;" +
                                       "Trusted_Connection=false;" +
                                       "database=db146732; " +
                                       "connection timeout=30");
        private static DBHelper instance = new DBHelper();
        private DBHelper() { }

        public DBHelper GetInstance() {
            return instance;
        }

        public static string Log(string query)
        {
            string log = "["+DateTime.Now.ToUniversalTime().ToString()+"]\n----------------------------------------------------\n\n{0}\n\n-----------------------------------------\n";
            Console.WriteLine(String.Format(log,query));
            return query;
        }

        public static System.Data.SqlClient.SqlConnection getConnection()
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
            }
            return conn;
        }
    }
}
