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
    public partial class FormPayment : Form
    {
        SqlConnection conn = DBHelper.getConnection();
        DualNumerator dualNumerator;
        private object[][] datas;
        private int max = 0;
        string[] columns = { "id_forma", "nazwa", "wymaganie_potwierdzenie", "serwis_posredniczacy", "dostepnosc", "dostepnosc_do" };

        public new void Close() {
            conn.Close();
            base.Close();
        }

        public FormPayment()
        {
            InitializeComponent();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            //>
            if (dualNumerator.HasNext()) {
                initTexts(dualNumerator.Next());
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            //<

            if (dualNumerator.HasPrevious()) {
                initTexts(dualNumerator.Previous());
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            //<<
            initTexts(dualNumerator.GetFirst());
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //>>
            initTexts(dualNumerator.GetLast());
        }

        private void button29_Click(object sender, EventArgs e)
        {
            //+
            initTexts(new object[] { "", "", 0, "", 0, DateTime.Now });
            this.button34.Visible = false;
            this.button1.Visible = true;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            //x
            object[] dataRow = dualNumerator.GetCurrent();
            if (dataRow.Length == 0) {
                return;
            }

            string id = dataRow[0].ToString();
            string delcmd = String.Format("DELETE FROM [Formy_płatności] WHERE id_forma={0}", id);
            DBHelper.Log(delcmd);
            SqlCommand sqlcmd = new SqlCommand(delcmd, conn);
            sqlcmd.ExecuteNonQuery();
            initData();
        }

        private void FormPayment_Load(object sender, EventArgs e)
        {
            this.comboBox4.Items.Add("Nie");
            this.comboBox4.Items.Add("Tak");
            this.comboBox5.Items.Add("Nie");
            this.comboBox5.Items.Add("Tak");
            this.button1.Visible = false;
            initData();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            //edycja
            object[] row = dualNumerator.GetCurrent();
            string nazwa = this.textBox10.Text;
            string serwis = this.textBox11.Text;
            int wymaga_potwierdzenia = this.comboBox4.SelectedIndex;
            int dostepnosc = this.comboBox5.SelectedIndex;
            string dos = this.dateTimePicker1.Value.ToString("s");
            string id_forma = row[0].ToString();
            string cmd = String.Format("UPDATE [Formy_płatności] SET nazwa='{0}',"+
                                    " serwis_posredniczacy='{1}',dosteposc={2},"+
                                    "wymaga_potwierdzenia={3},dostepnosc_do='{4}' "+
                                    " WHERE id_forma={5}", nazwa,
                                    serwis, dostepnosc,wymaga_potwierdzenia,
                                    dos, id_forma);
            DBHelper.Log(cmd);

            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            sqlcmd.ExecuteNonQuery();
            initData();
        }


        public void initData()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Formy_płatności]", conn);
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

        public void initTexts(object[] dataRow)
        {
            if (dataRow.Length == 0) {
                return;
            }
            this.textBox10.Text = dataRow[1].ToString();
            this.comboBox4.SelectedIndex = Convert.ToInt32(dataRow[2].ToString());
            this.textBox11.Text = dataRow[3].ToString();
            this.comboBox5.SelectedIndex = Convert.ToInt32(dataRow[4].ToString());
            this.dateTimePicker1.Value = Convert.ToDateTime(dataRow[5].ToString());
            //   this.dateTimePicker2.Value = Convert.ToDateTime(dataRow[1].ToString());
         //   this.dateTimePicker3.Value = Convert.ToDateTime(dataRow[2].ToString());
         //   this.textBox9.Text = dataRow[3].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nazwa = this.textBox10.Text;
            int wymagane = this.comboBox4.SelectedIndex;
            string serwis = this.textBox11.Text;
            int dostepnosc = this.comboBox5.SelectedIndex;
            string dos = this.dateTimePicker1.Value.ToString("s");
            string cmd = String.Format("INSERT INTO [Formy_płatności] VALUES('{0}',{1},'{2}',{3},'{4}')", nazwa, wymagane, serwis, dostepnosc, dos);
            DBHelper.Log(cmd);

            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            sqlcmd.ExecuteNonQuery();
            this.button1.Visible = false;
            this.button34.Visible = true;
            initData();
        }
    }
}
