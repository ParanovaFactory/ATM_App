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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=predator;Initial Catalog=Db_ATM;Integrated Security=True;TrustServerCertificate=True");

        private void btnLogin_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select personAccountNo, personPassword from TblPerson", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.GetString(0) == mskAccountNo.Text && rdr.GetString(1) == txtPassword.Text)
                {
                    Account account = new Account();
                    account.id = rdr.GetString(0);
                    account.Show();
                    this.Hide();
                }
            }
        }
    }
}
