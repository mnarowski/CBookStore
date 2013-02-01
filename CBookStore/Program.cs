using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace CBookStore
{
    static class Program
    {
        private static System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection("user id=st146732;" +
                                       "password=p146732;server=148.81.130.59;" +
                                       "Trusted_Connection=yes;" +
                                       "database=db146732; " +
                                       "connection timeout=30");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }


        public static System.Data.SqlClient.SqlConnection getConnection() {
            try {
                conn.Open();
            }
            catch (Exception e) {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
            }
            return conn;
        }
        public static string EncodePassword(string originalPassword)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(originalPassword);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}
