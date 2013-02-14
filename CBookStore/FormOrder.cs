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
    public partial class FormOrder : Form
    {
        SqlConnection conn = DBHelper.getConnection();
        DualNumerator dualNumerator;
        int max = 0;
        object[][] datas;
        string[] columns={"id_zamowienie","id_user","koszt","id_forma","status_zamowienia"};
        public FormOrder()
        {
            InitializeComponent();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //>
            if (dualNumerator.HasNext()) {
                initTexts(dualNumerator.Next());
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            //<
            if (dualNumerator.HasPrevious()) {
                initTexts(dualNumerator.Previous());
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            //<<
            initTexts(dualNumerator.GetFirst());
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //>>
            initTexts(dualNumerator.GetLast());
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //+
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //x
            object[] data = dualNumerator.GetCurrent();

            if (data.Length == 0) {
                return;
            }
            string id = data[0].ToString();
            string cmd = String.Format("DELETE FROM [dbo].[Zamowienia] WHERE id_zamowienie = {0}",id);
            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            sqlcmd.ExecuteNonQuery();
            initData();
        }

        public void initTexts(object[] data) {
            if (data.Length == 0) {
                return;
            }

            this.comboBox1.SelectedValue = Convert.ToInt32(data[1].ToString());
            this.textBox6.Text = data[2].ToString();
            this.comboBox2.SelectedValue = Convert.ToInt32(data[3].ToString());
            this.comboBox3.SelectedIndex = Convert.ToInt32(data[4].ToString());
            string id = data[0].ToString();
            DataTable dt;
            if (id.Equals(String.Empty))
            {
                SqlDataAdapter adapter2 = new SqlDataAdapter(String.Format("EXEC [dbo].ksiazki_w_zamowieniu {0}", id), conn);
                DataSet set2 = new DataSet();
                set2.Reset();
                adapter2.Fill(set2);
                dt = set2.Tables[0];
            }
            else {
                dt = new DataTable();
            } 
            this.checkedListBox1.DataSource = dt;
            this.checkedListBox1.DisplayMember = "tytul";
            this.checkedListBox1.ValueMember = "isbn";
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {

                checkedListBox1.SetItemChecked(i, true);

            }

        }

        public void initData()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Zamowienia]", conn);
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
                datas[i] = new object[columns.Length];
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

        private void FormOrder_Load(object sender, EventArgs e)
        {
            Auth a = Auth.GetInstance();
            if (!a.IsAdmin())
            {
                comboBox1.Visible = false;
            }
            else {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT id_user,imie+ ' ' + nazwisko as nazwa FROM [dbo].[Użytkownicy]", conn);
                DataSet set = new DataSet();
                set.Reset();
                adapter.Fill(set);

                DataTable datable = set.Tables["Table"];
                this.max = datable.Rows.Count;
                DBHelper.Log(this.max.ToString());
                string[] columns2 = { "id_user", "nazwa" };
                comboBox1.DisplayMember = "nazwa";
                comboBox1.ValueMember = "id_user";
                comboBox1.DataSource = datable;
            }

            SqlDataAdapter adapter2 = new SqlDataAdapter("SELECT id_forma, nazwa FROM [dbo].[Formy_płatności]", conn);
            DataSet set2 = new DataSet();
            set2.Reset();
            adapter2.Fill(set2);

            DataTable datable2 = set2.Tables["Table"];
            comboBox2.DisplayMember = "nazwa";
            comboBox2.ValueMember = "id_forma";
            comboBox2.DataSource = datable2;

            comboBox3.Items.Add("Nowe");
            comboBox3.Items.Add("W trakcie");
            comboBox3.Items.Add("Zrealizowane");
            initData();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            object[] c = dualNumerator.GetCurrent();
            if (c.Length == 0) {
                return;
            }
            string uzytkownik = this.comboBox1.SelectedValue.ToString();
            string cena = Convert.ToDouble(this.textBox6.Text).ToString(CultureInfo.InvariantCulture);
            string forma = this.comboBox2.SelectedValue.ToString();
            int status = this.comboBox3.SelectedIndex;
            string id = c[0].ToString();
            string cmd = String.Format("UPDATE [Zamowienia] SET id_user={0},koszt={1},id_forma={2},status_zamowienia={3} WHERE id_zamownienie={4}", uzytkownik, cena, forma, status,id);
            SqlCommand sqlcmd = new SqlCommand(cmd,conn);
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (!checkedListBox1.GetItemChecked(i)) {
                    DataRowView item = checkedListBox1.Items[i] as DataRowView;
                    var o = item.Row["isbn"];
                    string sqlcmdt = String.Format("DELETE FROM [Zamowienia_ksiazki] WHERE isbn={0} AND id_zamowienie={1}",o,id);
                    SqlCommand sqlcmdtc = new SqlCommand(sqlcmdt, conn);
                    sqlcmdtc.ExecuteNonQuery();
                }
            }

            initData();
        }
    }
}
