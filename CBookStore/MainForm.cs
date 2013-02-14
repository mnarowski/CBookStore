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
        private Form currForm;
        
        public MainForm()
        {
            InitializeComponent();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            Auth a = Auth.GetInstance();
            if (!a.IsAdmin()) {
                this.button3.Visible = false;
                this.button4.Visible = false;
                this.button5.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currForm != null)
            {
                currForm.Close();
            } 
           currForm = new FormBook();
           currForm.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currForm != null)
            {
                currForm.Close();
            } 
            currForm = new FormOrder();
            currForm.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currForm != null)
            {
                currForm.Close();
            } 
            currForm = new FormPromotion();
            currForm.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currForm != null)
            {
                currForm.Close();
            }
            currForm = new FormPayment();
            currForm.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (currForm != null)
            {
                currForm.Close();
            }
            currForm = new FormUser();
            currForm.Visible = true;
        }

        public new void Close() {
            Application.Exit();
        }

    }
}
