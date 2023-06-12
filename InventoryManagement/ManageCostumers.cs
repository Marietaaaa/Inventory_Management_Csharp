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
using System.Data.SqlClient;

namespace InventoryManagement
{
    public partial class ManageCostumers : Form
    {
        public ManageCostumers()
        {
            InitializeComponent();

        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Desktop\projekt c#\InventoryManagement\InventoryManagement\Marketdb.mdf;Integrated Security=True");
        void populate()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from CustomerTbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                CostumersGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            catch
            {

            }
        }

        private void timerDateAndTime_Tick(object sender, EventArgs e)
        {
            lblTimeAndDate.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
        }

        private void ManageCostumers_Load(object sender, EventArgs e)
        {
            timerDateAndTime.Start();
            populate();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }

        private void CostumersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CidTb.Text = CostumersGV.SelectedRows[0].Cells[0].Value.ToString();
            CnameTb.Text = CostumersGV.SelectedRows[0].Cells[1].Value.ToString();
            CmailTb.Text = CostumersGV.SelectedRows[0].Cells[2].Value.ToString();
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select Count(*) from OrderTbl where CustId= '" + CidTb.Text + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            OrderLabel.Text = dt.Rows[0][0].ToString();

            SqlDataAdapter sda1 = new SqlDataAdapter("select Sum(TotalAmt) from OrderTbl where CustId= '" + CidTb.Text + "'", Con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            AmountLabel.Text = dt1.Rows[0][0].ToString();

            SqlDataAdapter sda2 = new SqlDataAdapter("select Max(OrderDate) from OrderTbl where CustId= '" + CidTb.Text + "'", Con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            DateLabel1.Text = dt2.Rows[0][0].ToString();


            Con.Close();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO CustomerTbl VALUES (@CustId, @CustName, @CustMail)", Con);
                cmd.Parameters.AddWithValue("@CustId", CidTb.Text);
                cmd.Parameters.AddWithValue("@CustName", CnameTb.Text);
                cmd.Parameters.AddWithValue("@CustMail", CmailTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer added!");
                Con.Close();
                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CnameTb.Text == "")
            {
                MessageBox.Show("Shkruani emrin.");
            }
            else
            {
                Con.Open();
                string myquery = "DELETE FROM CustomerTbl WHERE CustName=@CustName";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.Parameters.AddWithValue("@CustName", CnameTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Klienti u fshi nga lista.");
                Con.Close();
                populate();
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE CustomerTbl SET CustName=@CustName, CustMail=@CustMail where CustId = @CustId", Con);
                cmd.Parameters.AddWithValue("@CustId", CidTb.Text);
                cmd.Parameters.AddWithValue("@CustName", CnameTb.Text);
                cmd.Parameters.AddWithValue("@CustMail", CmailTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Te dhenat u ndryshuan!");
                Con.Close();
                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CidTb_Enter(object sender, EventArgs e)
        {
            if (CidTb.Text == "Id Klientit")
            {
                CidTb.Text = "";
                CidTb.ForeColor = Color.Black;
            }

        }

        private void CidTb_Leave(object sender, EventArgs e)
        {
            if (CidTb.Text == "")
            {
                CidTb.Text = "Id Klientit";
                CidTb.ForeColor = Color.SlateGray;
            }

        }

        private void CnameTb_Enter(object sender, EventArgs e)
        {
            if (CnameTb.Text == "Emri Klientit")
            {
                CnameTb.Text = "";
                CnameTb.ForeColor = Color.SlateGray;
            }

        }

        private void CnameTb_Leave(object sender, EventArgs e)
        {
            if (CnameTb.Text == "")
            {
                CnameTb.Text = "Emri Klientit";
                CnameTb.ForeColor = Color.SlateGray;
            }

        }

        private void CmailTb_Enter(object sender, EventArgs e)
        {
            if (CmailTb.Text == "Email")
            {
                CmailTb.Text = "";
                CmailTb.ForeColor = Color.SlateGray;
            }

        }

        private void CmailTb_Leave(object sender, EventArgs e)
        {
            if (CmailTb.Text == "")
            {
                CmailTb.Text = "Email";
                CnameTb.ForeColor = Color.SlateGray;
            }

        }
    }
}

