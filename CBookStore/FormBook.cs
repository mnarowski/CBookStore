using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace CBookStore
{
    public partial class FormBook : Form
    {
        SqlConnection conn = DBHelper.getConnection();
        string[] columns = { "isbn", "tytul", "autor", "wydawca", "cena", "dostepnych"};
        int max = 0;
        
        private object[][] datas;
        DualNumerator dualNumerator;
        public FormBook()
        {
            InitializeComponent();
        }


        private void initForm(){
            label6.Visible = false;
            textBox6.Visible = false;
            button1.Visible = false;
            initData();
            Auth a = Auth.GetInstance();

            string selectedOrders = String.Format("SELECT * FROM [Zamowienia] WHERE status_zamowienia=0 ");
            if (!a.IsAdmin())
            {
                this.button11.Visible = false;
                this.button9.Visible = false;
                this.textBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.textBox3.Enabled = false;
                this.textBox4.Enabled = false;
                this.textBox5.Enabled = false;
                this.button2.Visible = false;
                selectedOrders += String.Format(" And id_user = {0}", a.selectedUserId);
            }

            SqlDataAdapter adapter = new SqlDataAdapter(selectedOrders, conn);
            DataSet set = new DataSet();
            set.Reset();
            adapter.Fill(set);

            DataTable datable = set.Tables["Table"];
            if (datable.Rows.Count > 0)
            {
                this.comboBox1.DisplayMember = "id_zamowienie";
                this.comboBox1.ValueMember = "id_zamowienie";
                this.comboBox1.DataSource = datable;
            }
            label7.Visible = false;
            comboBox1.Visible = false;
            button3.Visible = false;



            //promocje
            SqlDataAdapter adapter2 = new SqlDataAdapter("SELECT id_promocja,data_poczatek FROM [Promocje]", conn);
            DataSet set2 = new DataSet();
            set2.Reset();
            adapter2.Fill(set2);

            DataTable datable2 = set2.Tables["Table"];
            if (datable2.Rows.Count > 0)
            {
                this.comboBox2.DisplayMember = "data_poczatek";
                this.comboBox2.ValueMember = "id_promocja";
                this.comboBox2.DataSource = datable2;
            }
            label8.Visible = false;
            comboBox2.Visible = false;
            button4.Visible = false;
        }

        private void FormBook_Load(object sender, EventArgs e)
        {
            initForm();

        }

        public void initData() {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Książki]", conn);
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
                datas[i] = new object[6];
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

        public void initTexts(object[] reader) {
            if (reader.Length == 0)
            {
                return;
            }
            this.textBox1.Text = reader[1].ToString();
            this.textBox2.Text = reader[2].ToString();
            this.textBox3.Text = reader[3].ToString();
            this.textBox4.Text = Convert.ToString(Convert.ToDouble(reader[4]));
            this.textBox5.Text = reader[5].ToString();
            label10.Text = String.Empty;
            if (!reader[0].ToString().Equals(String.Empty))
            {
                string test = String.Format("SELECT [dbo].cena_promocyjna('{0}')",reader[0].ToString());
                SqlCommand cmd = new SqlCommand(test, conn);
                string text = String.Empty;
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows) {
                    r.Read();
                    text = r.GetDecimal(0).ToString(CultureInfo.InvariantCulture);
                }
                r.Close();
                label10.Text = text;
            }
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
            //+
            initTexts(new object[6] { "", "", "", "", 1.0, 0 });
            label6.Visible = true ;
            textBox6.Visible = true;
            button1.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //x
            object[] current = dualNumerator.GetCurrent();
            if (current.Length == 0) {
                return;
            }
            if (current != null) {
                string isbn = current[0].ToString();
                string sqlcmd = String.Format("DELETE FROM [Książki] WHERE isbn='{0}'", isbn);
                DBHelper.Log(sqlcmd);
                SqlCommand sql = new SqlCommand(sqlcmd, conn);
                sql.ExecuteNonQuery();
                initData();
            }
        }

        public new void Close() {
            conn.Close();
            base.Close();
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dualNumerator.HasNext())
            {
                initTexts(dualNumerator.Next());
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            //edycja
            object[] data = dualNumerator.GetCurrent();
            string tytul = this.textBox1.Text;
            string autor = this.textBox2.Text;
            string wydawca = this.textBox3.Text;
            
            double cena = Convert.ToDouble(this.textBox4.Text);
            decimal ilosc_dostepnych = Int32.Parse(this.textBox5.Text);
            string isbn = data[0].ToString();
            string command = String.Format("UPDATE [Książki] SET tytul='{0}', autor='{1}', wydawca='{2}',cena={3}, dostepnych={4} WHERE isbn='{5}'",tytul,autor,wydawca,cena.ToString(CultureInfo.InvariantCulture),ilosc_dostepnych,isbn);
            DBHelper.Log(command);
            SqlCommand sqlcmd = new SqlCommand(command, conn);
            sqlcmd.ExecuteNonQuery();
            initData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //dodanie do zamówienia
            object[] param = dualNumerator.GetCurrent();
            if (param.Length == 0) {
                System.Windows.Forms.MessageBox.Show("Nie można dodać tworzonej/pustej książki do zamówienia");
                return;
            }

            if (param[0].ToString().Equals(String.Empty)) {
                System.Windows.Forms.MessageBox.Show("Nie można dodać tworzonej/pustej książki do zamówienia");
                return;
            }

            label7.Visible = true;
            comboBox1.Visible = true;
            button3.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tytul = this.textBox1.Text;
            string autor = this.textBox2.Text;
            string wydawca = this.textBox3.Text;

            double cena = Convert.ToDouble(this.textBox4.Text);
            decimal ilosc_dostepnych = Int32.Parse(this.textBox5.Text);
            string isbn = textBox6.Text.ToString().Trim();
            if (isbn.Length != 11) {
                System.Windows.Forms.MessageBox.Show("Numer isbn musi zawierać 11 znaków");
                return;
            }
            string strcmd = String.Format("SELECT * FROM [Książki] WHERE isbn='{0}'", isbn);
            DBHelper.Log(strcmd);
            SqlCommand cmd = new SqlCommand(strcmd, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                reader.Close();
                System.Windows.Forms.MessageBox.Show("Numer isbn nie jest unikalny");
                return;
            }
            reader.Close();

            string sqlinsert = String.Format("INSERT INTO [Książki] VALUES ('{0}','{1}','{2}','{3}',{4},{5})",isbn,tytul,autor,wydawca,cena.ToString(CultureInfo.InvariantCulture),ilosc_dostepnych);
            SqlCommand sqlcmd = new SqlCommand(DBHelper.Log(sqlinsert), conn);
            sqlcmd.ExecuteNonQuery();
            label6.Visible = false;
            textBox6.Visible = false;
            button1.Visible = false;

            initData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //dodaj do promocji
            this.label8.Visible = true;
            this.comboBox2.Visible = true;
            this.button4.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // dodaj do zamówienia
            object[] curr = dualNumerator.GetCurrent();

            if (curr.Length == 0) {
                System.Windows.Forms.MessageBox.Show("Najpierw zapis książke");
                return;
            }
            int val = 0;
            if (this.comboBox1.SelectedValue != null)
            { val = Convert.ToInt32(this.comboBox1.SelectedValue.ToString()); }
            if (val == 0) {
                int userid = Auth.GetInstance().selectedUserId;
                SqlCommand sqlcmd = new SqlCommand(String.Format("INSERT INTO [dbo].[Zamowienia]" +
                    " VALUES({0},0,1 ,0)",userid), conn);
                sqlcmd.ExecuteNonQuery();
                SqlCommand sqlcmdid = new SqlCommand(String.Format("SELECT id_zamowienie FROM [Zamowienia] WHERE id_user = {0}", userid), conn);
                SqlDataReader reader = sqlcmdid.ExecuteReader();
                reader.Read();
                val = reader.GetInt32(0);
                reader.Close();
            }
            string isbn = curr[0].ToString();
            SqlCommand test = new SqlCommand(String.Format("SELECT * FROM [Zamowienie_ksiazki] WHERE isbn='{0}' AND id_zamowienie={1}", isbn, val), conn);
            SqlDataReader dr = test.ExecuteReader();
            bool has = dr.HasRows;
            dr.Close();
            if (has)
            {
                System.Windows.Forms.MessageBox.Show("Książki jest już najprawodpodobniej w zamówieniu");
                return;
            }

            SqlCommand sqlcmdx = new SqlCommand(String.Format("INSERT INTO [dbo].[Zamowienie_ksiazki](isbn,id_zamowienie)" +
                    " VALUES('{0}',{1})", isbn,val), conn);
            sqlcmdx.ExecuteNonQuery();
            initForm();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // dodaj do zamówienia
            object[] curr = dualNumerator.GetCurrent();

            if (curr.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Najpierw zapis książke");
                return;
            }

            int val = Convert.ToInt32(this.comboBox2.SelectedValue.ToString());

            if (val == 0)
            {
                System.Windows.Forms.MessageBox.Show("Brak promocji");
                return;
            }


            string isbn = curr[0].ToString();
            SqlCommand test = new SqlCommand(String.Format("SELECT * FROM [Ksiazki_promocje] WHERE isbn='{0}' AND id_promocja={1}",isbn,val), conn);
            SqlDataReader dr = test.ExecuteReader();
            bool has = dr.HasRows;
            dr.Close();
            if (has) {
                System.Windows.Forms.MessageBox.Show("Książki jest już najprawodpodobniej w promocji");
                return;
            }
            SqlCommand sqlcmdx = new SqlCommand(DBHelper.Log(String.Format("INSERT INTO [Ksiazki_promocje](isbn,id_promocja)" +
                    " VALUES('{0}',{1})", isbn, val)), conn);
            sqlcmdx.ExecuteNonQuery();
            initForm();

        }

        
    }
}
