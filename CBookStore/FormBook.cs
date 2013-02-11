﻿using System;
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
    public partial class FormBook : Form
    {
        SqlConnection conn = DBHelper.getConnection();
        string[] columns = { "isbn", "tytul", "autor", "wydawca", "cena", "dostepnych" };
        int max = 0;
        int min = 0;
        int curr = 0;
        
        private object[][] datas;
        DualNumerator dualNumerator;
        public FormBook()
        {
            InitializeComponent();
        }

        private int selectedIndex;

        private void FormBook_Load(object sender, EventArgs e)
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
            foreach(DataRow dr in datable.Rows) {
                datas[i] = new object[6];
                int v = 0;
                foreach(string p in columns){
                        datas[i][v] = dr[p];
                        v++;
                    }
                i++;
            }

            dualNumerator = new DualNumerator(datas, max);
            initTexts(dualNumerator.GetFirst());
        }

        public void initTexts(object[] reader) {
            this.textBox1.Text = reader[1].ToString();
            this.textBox2.Text = reader[2].ToString();
            this.textBox3.Text = reader[3].ToString();
            this.textBox4.Text = Convert.ToString(Convert.ToDouble(reader[4]));
            this.textBox5.Text = reader[5].ToString();

        }

        private void button12_Click(object sender, EventArgs e)
        {
            

            if (dualNumerator.HasPrevious())
            {
                initTexts(dualNumerator.Previous());
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            initTexts(dualNumerator.GetFirst());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            initTexts(dualNumerator.GetLast());
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        public void Close() {
            base.Close();
            conn.Close();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dualNumerator.HasNext())
            {
                Console.WriteLine("Im herre");
                initTexts(dualNumerator.Next());
            }
        }
    }
}
