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
        private string[] columns = { "email","nazwisko","imie","rola","data_urodzenia","miasto","ulica","nr_budynku","nr_lokalu","kod_pocztowy","id_user"};
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
            button1.Visible = true;
            button35.Visible = false;
            initTexts(new object[] { "","","",0,DateTime.Now,"","",0,0,""});
        }

        private void button39_Click(object sender, EventArgs e)
        {
            //x
            object[] cur = dualNumerator.GetCurrent();

            if (cur.Length < 1)
            {
                return;
            }
            string id = cur[10].ToString();
            string cmd = String.Format("DELETE FROM [dbo].[Użytkownicy] WHERE id_user = {0}", id);
            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            sqlcmd.ExecuteNonQuery();

            initData();
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            this.comboBox6.Items.Add("Użytkownik");
            this.comboBox6.Items.Add("Pracownik");
            this.comboBox6.Items.Add("Admin");
            button1.Visible = false;
            initData();    
        }


        public void initLoaded(){
            
        }


        private void FormUser_Leave(object sender, EventArgs e)
        {
            //conn.Close();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            //edycja
            string email = this.textBox7.Text;
            string nazwisko = this.textBox8.Text;
            string imie = this.textBox12.Text;
            int rola = this.comboBox6.SelectedIndex;
            string date = this.dateTimePicker4.Value.ToString("s");
            string miasto = this.textBox14.Text;
            string ulica = this.textBox15.Text;
            int budynek = Convert.ToInt32(this.textBox17.Text);
            int lokal = Convert.ToInt32(this.textBox16.Text);
            string kod = this.textBox13.Text;
            object[] cur = dualNumerator.GetCurrent();

            if (cur.Length < 1) {
                return;
            }
            string id = cur[10].ToString();
            string cmd = String.Format("UPDATE [dbo].[Użytkownicy] SET" +
            "email='{0}',nazwisko='{1}',imie='{2}',rola={3},data_urodzenia='{4}',"+
            "miasto='{5}',ulica='{6}',nr_budynku={7},nr_lokalu={8},kod_pocztowy+'{9}' WHERE id_user={10}",
            email, nazwisko, imie, rola, date, miasto, ulica, budynek, lokal, kod);

            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            sqlcmd.ExecuteNonQuery();
            initData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dodaj
            //var p = new object[] { "email",
            //                       "nazwisko",
            //                       "imie",
            //                       "rola",
            //                       "data_urodzenia",
            //                       "miasto",
            //                       "ulica",
            //                       "nr_budynku",
            //                       "nr_lokalu",
            //                       "kod_pocztowy",
            //                       "id_user"};

            string email =this.textBox7.Text ;
            string nazwisko = this.textBox8.Text ;
            string imie = this.textBox12.Text;
            int rola = this.comboBox6.SelectedIndex ;
            string date = this.dateTimePicker4.Value.ToString("s");
            string miasto = this.textBox14.Text ;
            string ulica = this.textBox15.Text;
            int budynek = Convert.ToInt32(this.textBox17.Text);
            int lokal = Convert.ToInt32(this.textBox16.Text);
            string kod = this.textBox13.Text ;
            string cmd = String.Format("INSERT INTO [dbo].[Użytkownicy]"+ 
            "VALUES('{0}','{1}','{2}',{3},'{4}','{5}','{6}',{7},{8},'{9}')",
            email,nazwisko,imie,rola,date,miasto,ulica,budynek,lokal,kod);

            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            sqlcmd.ExecuteNonQuery();
            this.button35.Visible = true;
            this.button1.Visible = false;
            initData();
        }

        public void initData()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [dbo].[Użytkownicy]", conn);
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

        public new void Close() {
            conn.Close();
            base.Close();
        }
    }
}
