using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;

namespace CBookStore
{
    static class Program
    {
        

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

       
    }
}
