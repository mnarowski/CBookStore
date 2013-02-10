using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.DataTable;
using System.Data;
using CBookStore.Utils;
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

        public bool hasNext() {
            return false;
        }

        public bool hasPrevious() {
            return false;
        }

        public System.Data.DataRow getCurrentRow() {
            return null;
        }

        public System.Data.DataRow getNextRow() {
            return null;
        }

        public System.Data.DataRow getPreviousRow() {
            return null;
        }

        public void deleteCurrent() { 
            
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
