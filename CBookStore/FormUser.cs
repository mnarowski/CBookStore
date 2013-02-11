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
        private SqlDataReader reader = null;

        public FormUser()
        {
            InitializeComponent();
        }

        private void button40_Click(object sender, EventArgs e)
        {

        }

        private void button42_Click(object sender, EventArgs e)
        {

        }

        private void button43_Click(object sender, EventArgs e)
        {

        }

        private void button38_Click(object sender, EventArgs e)
        {

        }

        private void button41_Click(object sender, EventArgs e)
        {

        }

        private void button39_Click(object sender, EventArgs e)
        {

        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            initData();    
        }

        public void initData() {
            SqlCommand adapter = new SqlCommand("SELECT * FROM [dbo].[Użytkownicy]", conn);
            reader = adapter.ExecuteReader();
            initLoaded();
        }

        public void initLoaded(){
            if(reader.Read()){
                this.textBox12.Text = reader.GetString(0);
                this.textBox13.Text = reader.GetString(1);
            }
        }


        private void FormUser_Leave(object sender, EventArgs e)
        {
            conn.Close();
        }
    }
}
