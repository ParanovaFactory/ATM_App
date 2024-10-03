using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{
    public partial class Account : Form
    {
        public Account()
        {
            InitializeComponent();
        }

        public string id;
        SqlConnection conn = new SqlConnection(@"Data Source=predator;Initial Catalog=Db_ATM;Integrated Security=True;TrustServerCertificate=True");

        void list()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from TblAccount where AccountNo = @p1",conn);
            cmd.Parameters.AddWithValue("@p1", id);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();

            conn.Open();
            SqlCommand cmd1 = new SqlCommand("select * from TblOperation where sender = @p1 or receiver = @p2", conn);
            cmd1.Parameters.AddWithValue("@p1", id);
            cmd1.Parameters.AddWithValue("@p2", id);
            SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            adapter1.Fill(dt1);
            dataGridView2.DataSource = dt1;
            conn.Close();
        }

        private void Account_Load(object sender, EventArgs e)
        {
            list();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            decimal budget =0;
            conn.Open();
            SqlCommand cmd2 = new SqlCommand("select Budget from TblAccount where AccountNo = @p5", conn);
            cmd2.Parameters.AddWithValue("@p5", id);
            SqlDataReader reader = cmd2.ExecuteReader();
            while (reader.Read())
            {
                budget = reader.GetDecimal(0);
            }
            reader.Close();

            SqlCommand cmd = new SqlCommand("insert into TblOperation (sender,receiver,amount,operationType) values (@p1,@p2,@p3,@p4)", conn);
            cmd.Parameters.AddWithValue("@p1", id);
            cmd.Parameters.AddWithValue("@p2", mscReceiverNo.Text);
            cmd.Parameters.AddWithValue("@p3", decimal.Parse(txtAmount.Text));
            cmd.Parameters.AddWithValue("@p4", cmbOperation.Text);

            if (budget >= decimal.Parse(txtAmount.Text))
            {
                cmd.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand("update TblAccount set Budget -=@p6 where AccountNo = @p7", conn);
                cmd3.Parameters.AddWithValue("@p6", txtAmount.Text);
                cmd3.Parameters.AddWithValue("@p7", id);
                cmd3.ExecuteNonQuery();


                SqlCommand cmd4 = new SqlCommand("update TblAccount set Budget +=@p8 where AccountNo = @p9", conn);
                cmd4.Parameters.AddWithValue("@p8", txtAmount.Text);
                cmd4.Parameters.AddWithValue("@p9", mscReceiverNo.Text);
                cmd4.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Chech the value");
            }

            conn.Close();
            list();
        }
    }
}
