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
    public partial class MainForm : Form
    {
        private string selectUsers = "SELECT * FROM [dbo].[Użytkownicy];";
        private string selectBooks = "Select * FROM [dbo].[Książki];";
        private string selectOrders = "SELECT * FROM [dbo].[Zamówienia];";
        private string selectPayments = "SELECT * FROM [dbo].[Formy_płatności];";
        private string selectPromotions = "SELECT * FROM [dbo].[Promocje];";
        
        public MainForm()
        {
            InitializeComponent();
        }

        private DataSet myData;
        private DataTable table;

        private void initData() {
            myData = new DataSet();
            myData.Reset();
            SqlConnection conn = DBHelper.getConnection();
            SqlDataAdapter abooks = new SqlDataAdapter(DBHelper.Log(selectBooks), conn);
            abooks.Fill(myData);
            SqlDataAdapter ausers = new SqlDataAdapter(DBHelper.Log(selectUsers), conn);
            ausers.Fill(myData);
            SqlDataAdapter apromotion = new SqlDataAdapter(DBHelper.Log(selectPromotions), conn);
            apromotion.Fill(myData);
            SqlDataAdapter apayments = new SqlDataAdapter(DBHelper.Log(selectPayments), conn);
            apayments.Fill(myData);
            SqlDataAdapter aorders = new SqlDataAdapter(DBHelper.Log(selectOrders), conn);
            aorders.Fill(myData);
        }

        
        private void MainForm_Load(object sender, EventArgs e)
        {
            initData();
            Auth auth = Auth.GetInstance();
            if ( !auth.IsAdmin() )
            {
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = null;
                this.tabPage5.Parent = null;
            }
            else {
                this.button7.Visible = false;
            }

            table = myData.Tables["Książki"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBHelper.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            
            //>
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //<
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //>>
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //<<
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //+
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //x
        }

        private void button32_Click(object sender, EventArgs e)
        {
            //edycja książki
        }

        private void button31_Click(object sender, EventArgs e)
        {
            //edycja zamówienia
        }

        private void button33_Click(object sender, EventArgs e)
        {
            //edycja promocji
        }

        private void button36_Click(object sender, EventArgs e)
        {
            //dodanie ksiazki do zamówienia
        }

        private void button34_Click(object sender, EventArgs e)
        {
            //edycja formy płatności
        }

        private void button35_Click(object sender, EventArgs e)
        {
            //edycja użytkownika
        }

        private void TabIndexChanged1(object sender, EventArgs e) {
            string pstr = this.tabControl1.SelectedTab.Text;
            //zmiana zakładki
        }

        

    }
}
