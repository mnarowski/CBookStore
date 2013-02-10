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

        private Dictionary<String, String> mappedSet = new Dictionary<String, String>();

        private Navigation nNavigation = Navigation.GetInstance();

        private void MainForm_Load(object sender, EventArgs e)
        {
            mappedSet.Add("Książki","Książki");
            mappedSet.Add("Użytkownicy", "Użytkownicy");
            mappedSet.Add("Zamówienia","Zamówienia");
            mappedSet.Add("Promocje", "Promocje");
            mappedSet.Add("Formy płatności", "Formy płatności");


            Auth auth = Auth.GetInstance();
            if ( !auth.IsAdmin() ) {
                this.tabPage3.Hide();
                this.tabPage4.Hide();
                this.tabPage5.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
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

        private void TabIndexChanged(object sender, EventArgs e) {
            string pstr = this.tabControl1.SelectedTab.Text;
            nNavigation.Reload(pstr);
            //zmiana zakładki
        }
    }
}
