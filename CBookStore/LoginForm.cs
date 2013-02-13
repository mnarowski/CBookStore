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

            string strcommand = String.Format("Select * FROM [dbo].[Użytkownicy] u where email = '{0}' AND [dbo].MD5(u.nazwisko) = [dbo].MD5('{1}')", this.textBox1.Text, this.textBox2.Text);
            Console.WriteLine(strcommand);
            SqlCommand sqlcmd = new SqlCommand(strcommand,con);
            SqlDataReader reader = null;
            reader = sqlcmd.ExecuteReader();

            
            if (reader.HasRows)
            {
                Auth a = Auth.GetInstance();
                reader.Read();
                int role= reader.GetInt32(3);
                a.selectedUserId = reader.GetInt32(10);
                a.setIfIsWorker(role);
                con.Close();
                MainForm mForm = new MainForm();
                mForm.Visible = true;

            }
            else {
                con.Close();
                System.Windows.Forms.MessageBox.Show("Niepoprawny login lub hasło");
            }
        }
    }
}
