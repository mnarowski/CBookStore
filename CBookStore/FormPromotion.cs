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
    public partial class FormPromotion : Form
    {
        string[] columns = { "id_promocja", "data_poczatek", "data_koniec", "obnizka"};
        SqlConnection conn = DBHelper.getConnection();
        int max = 0;
        object[][] datas;
        DualNumerator dualNumerator;
        public FormPromotion()
        {
            InitializeComponent();
        }


        public void initData()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Promocje]", conn);
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

        public void initTexts(object[] dataRow) {
            if (dataRow.Length == 0)
            {
                return;
            }
            this.dateTimePicker2.Value = Convert.ToDateTime(dataRow[1].ToString());
            this.dateTimePicker3.Value = Convert.ToDateTime(dataRow[2].ToString());
            this.textBox9.Text = dataRow[3].ToString();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (dualNumerator.HasNext()) {
                initTexts(dualNumerator.Next());
            }
            //>
        }

        private void button24_Click(object sender, EventArgs e)
        {
            //<
            if (dualNumerator.HasPrevious()) {
                initTexts(dualNumerator.Previous());
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            //<<
            initTexts(dualNumerator.GetFirst());
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //>>
            initTexts(dualNumerator.GetLast());
        }

        private void button23_Click(object sender, EventArgs e)
        {
            //+
            this.button33.Visible = false;
            initTexts(new object[4] { "", null, null, 0 });
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //x
            object[] current = dualNumerator.GetCurrent();
            if (current.Length > 0)
            {
                string id_promotion = current[0].ToString();
                string sqlcmd = String.Format("DELETE FROM [Promocje] WHERE id_promocja={0}", id_promotion);
                DBHelper.Log(sqlcmd);
                SqlCommand sql = new SqlCommand(sqlcmd, conn);
                sql.ExecuteNonQuery();
                initData();
            }
        }

        public new void Dispose() {
            conn.Close();
            base.Dispose();
        }

        private void FormPromotion_Load(object sender, EventArgs e)
        {
            initData();
            this.button1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlcmd = String.Format("INSERT INTO [Promocje] VALUES({0},{1},{2})", this.dateTimePicker2.Value.ToString(CultureInfo.InvariantCulture), this.dateTimePicker3.Value.ToString(CultureInfo.InvariantCulture), this.textBox9.Text.ToString(CultureInfo.InvariantCulture));
            DBHelper.Log(sqlcmd);

            SqlCommand sql = new SqlCommand(sqlcmd, conn);
            sql.ExecuteNonQuery();
            this.button1.Visible = false;
            this.button33.Visible = true;
            initData();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            //edycja
            object[] data = dualNumerator.GetCurrent();
            string date_poczatek = this.dateTimePicker2.Value.ToUniversalTime().ToString("s");
            string date_koniec = this.dateTimePicker3.Value.ToUniversalTime().ToString("s");
            string obnizka = this.textBox9.Text;
            string id = data[0].ToString();
            string command = String.Format("UPDATE [Promocje] SET data_poczatek='{0}', data_koniec='{1}', obnizka={2} WHERE id_promocja='{3}'", date_poczatek, date_koniec,  obnizka.ToString(CultureInfo.InvariantCulture), id);
            DBHelper.Log(command);
            SqlCommand sqlcmd = new SqlCommand(command, conn);
            sqlcmd.ExecuteNonQuery();
            initData();
        }
    }
}
