using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CBookStore
{
    class Navigation
    {
        private DataTable TableInfo;

        private String Table{set;get;}
        
        private int LastId{set;get;}

        private int currentIndex;

        private static Navigation Instance;
        
        public static Navigation GetInstance() {
            if (Instance == null) {
                Instance = new Navigation();
            }

            return Instance;
        }

        public IEnumerable<System.Data.DataRow> GetEnumerable() {
            return DataTableExtensions.AsEnumerable(TableInfo);
        }

        public void Reload(string tblName) {
            SqlConnection sqlcon = DBHelper.getConnection();
            string str = String.Format("SELECT * FROM [{0}]",tblName);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(str, sqlcon);
            DataSet ds = new DataSet();
            ds.Reset();
            sqlAdapter.Fill(ds);
            TableInfo = ds.Tables[tblName];
            this.Table = tblName;
            sqlcon.Close();
        }
    }
}
