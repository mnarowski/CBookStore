using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CBookStore
{
    public partial class FormOrder : Form
    {
        SqlConnection conn = DBHelper.getConnection();
        DualNumerator dualNumerator;
        int max = 0;
        object[][] datas;
        string[] columns={"id_zamowienie","id_user","koszt","id_forma","status_zamowienia"};
        public FormOrder()
        {
            InitializeComponent();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //>
            if (dualNumerator.HasNext()) {
                initTexts(dualNumerator.Next());
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            //<
            if (dualNumerator.HasPrevious()) {
                initTexts(dualNumerator.Previous());
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            //<<
            initTexts(dualNumerator.GetFirst());
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //>>
            initTexts(dualNumerator.GetLast());
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //+
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //x
            object[] data = dualNumerator.GetCurrent();

            if (data.Length == 0) {
                return;
            }
            string id = data[0].ToString();
            string cmd = String.Format("DELETE FROM [dbo].[Zamowienia] WHERE id_zamowienie = {0}",id);
            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            sqlcmd.ExecuteNonQuery();
            initData();
        }

        public void initTexts(object[] data) { 
            
        }

        public void initData()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Zamowienia]", conn);
            DataSet set = new DataSet();
            set.Reset();
            adapter.Fill(set);

            DataTable datable = set.Tables["Table"];
            int i = 0;
            this.max = datable.Rows.Count;
            DBHelper.Log(this.max.ToString());
            datas = new object[max][];
            foreach (DataRow dr in datable.Rows)
            {
                datas[i] = new object[columns.Length];
                int v = 0;
                foreach (string p in columns)
                {
                    datas[i][v] = dr[p];
                    v++;
                }
                i++;
            }

            dualNumerator = new DualNumerator(datas, max);
            initTexts(dualNumerator.GetFirst());

        }

        private void FormOrder_Load(object sender, EventArgs e)
        {
            initData();
        }
    }
}
