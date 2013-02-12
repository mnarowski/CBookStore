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
    public partial class FormUser : Form
    {
        private SqlConnection conn= DBHelper.getConnection();
        private DualNumerator dualNumerator;
        private int max = 0;
        private object[][] datas;
        private string[] columns = { };
        public FormUser()
        {
            InitializeComponent();
        }

        private void button40_Click(object sender, EventArgs e)
        {
            //>
        }

        private void button42_Click(object sender, EventArgs e)
        {
            //<
        }

        private void button43_Click(object sender, EventArgs e)
        {
            //<<
        }

        private void button38_Click(object sender, EventArgs e)
        {
            //>>
        }

        private void button41_Click(object sender, EventArgs e)
        {
            //+
        }

        private void button39_Click(object sender, EventArgs e)
        {
            //x
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            initData();    
        }


        public void initLoaded(){
            
        }


        private void FormUser_Leave(object sender, EventArgs e)
        {
            conn.Close();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            //edycja
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dodaj
        }

        public void initData()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Książki]", conn);
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
                datas[i] = new object[6];
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

        public void initTexts(object[] reader)
        {
            if (reader.Length == 0)
            {
                return;
            }
            //this.textBox1.Text = reader[1].ToString();
            //this.textBox2.Text = reader[2].ToString();
            //this.textBox3.Text = reader[3].ToString();
            //this.textBox4.Text = Convert.ToString(Convert.ToDouble(reader[4]));
            //this.textBox5.Text = reader[5].ToString();

        }
    }
}
