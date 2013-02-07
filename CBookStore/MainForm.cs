using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CBookStore
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Auth auth = Auth.GetInstance();
            if ( !auth.IsAdmin() ) {
                this.tabPage3.Hide();
                this.tabPage4.Hide();
                this.tabPage5.Hide();
            }
        }
    }
}
