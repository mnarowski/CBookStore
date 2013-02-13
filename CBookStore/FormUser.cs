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
            if (dualNumerator.HasNext()) {
                initTexts(dualNumerator.Next());
            }
        }

        private void button42_Click(object sender, EventArgs e)
        {
            //<
            if (dualNumerator.HasPrevious()) {
                initTexts(dualNumerator.Previous());
            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            //<<
            initTexts(dualNumerator.GetFirst());
        }

        private void button38_Click(object sender, EventArgs e)
        {
            //>>
            initTexts(dualNumerator.GetLast());
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
            this.comboBox6.Items.Add("Użytkownik");
            this.comboBox6.Items.Add("Pracownik");
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
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Użytkownicy]", conn);
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
                datas[i] = new object[11];
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
            DBHelper.Log(reader.Length.ToString());
            if (reader.Length == 0)
            {
                return;
            }
            this.textBox7.Text = reader[0].ToString();
            this.textBox8.Text = reader[1].ToString();
            this.textBox12.Text = reader[2].ToString();
            this.comboBox6.SelectedIndex = Convert.ToInt32(reader[3].ToString());
            this.dateTimePicker4.Value = Convert.ToDateTime(reader[4].ToString());
            this.textBox14.Text = reader[5].ToString();
            this.textBox15.Text = reader[6].ToString();
            this.textBox17.Text = reader[7].ToString();
            this.textBox16.Text = reader[8].ToString();
            this.textBox13.Text = reader[8].ToString();
        }
    }
}
