using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace CBookStore
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = DBHelper.getConnection();
            string strcommand = String.Format("Select * FROM [dbo].[Użytkownicy] where email = {0} AND MD5(nazwisko) = {1}", this.textBox1.Text, Program.EncodePassword(this.textBox2.Text));
            SqlCommand sqlcmd = new SqlCommand(strcommand,con);
            SqlDataReader reader = sqlcmd.ExecuteReader();
            if (reader.HasRows)
            {
                MainForm mForm = new MainForm();
                mForm.Visible = true;
            }
            else {
                System.Windows.Forms.MessageBox.Show("Niepoprawny login lub hasło");
            }
        }
    }
}
