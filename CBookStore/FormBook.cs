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
        string[] columns = { "isbn", "tytul", "autor", "wydawca", "cena", "dostepnych" };
        int max = 0;
        
        private object[][] datas;
        DualNumerator dualNumerator;
        public FormBook()
        {
            InitializeComponent();
        }


        private void FormBook_Load(object sender, EventArgs e)
        {
            label6.Visible = false;
            textBox6.Visible = false;
            button1.Visible = false;
            initData();
            Auth a = Auth.GetInstance();
            if (!a.IsAdmin())
            {
                this.button11.Visible = false;
                this.button9.Visible = false;
                this.textBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.textBox3.Enabled = false;
                this.textBox4.Enabled = false;
                this.textBox5.Enabled = false;
            }
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
            base.Close();
            conn.Close();

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

        
    }
}
