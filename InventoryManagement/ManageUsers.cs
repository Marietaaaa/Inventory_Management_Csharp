using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryManagement
{
    public partial class ManageUsers : Form
    {
        public ManageUsers()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Documents\Marketdb.mdf;Integrated Security=True;Connect Timeout=30");
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        void populate()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from Usertbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder= new SqlCommandBuilder(da);
                var ds=new DataSet();
                da.Fill(ds);
                UsersGV.DataSource = ds.Tables[0];
                Con.Close();
                

            }
            catch 
            { 

            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            timerDateAndTime.Start();
            populate();
        }

        private void timerDateAndTime_Tick(object sender, EventArgs e)
        {
            lblTimeAndDate.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Usertbl VALUES (@Uname, @Fname, @Password, @Mail)", Con);
                cmd.Parameters.AddWithValue("@Uname", UnameTb.Text);
                cmd.Parameters.AddWithValue("@Fname", FnameTb.Text);
                cmd.Parameters.AddWithValue("@Password", PasswordTb.Text);
                cmd.Parameters.AddWithValue("@Mail", MailTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("User added!");
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
            if (MailTb.Text == "")
            {
                MessageBox.Show("Shkruani email.");
            }
            else 
            {
                Con.Open();
                string myquery = "delete from Usertbl where Umail='" + MailTb.Text + "';";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Perdoruesi u fshi.");
                Con.Close();
                populate(); 
            }
        }

        private void UsersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UnameTb.Text = UsersGV.SelectedRows[0].Cells[0].Value.ToString();
            FnameTb.Text = UsersGV.SelectedRows[0].Cells[1].Value.ToString();
            PasswordTb.Text = UsersGV.SelectedRows[0].Cells[2].Value.ToString();
            MailTb.Text = UsersGV.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Usertbl SET Uname=@Uname, Ufullname=@Fname, UPassword=@Password WHERE Umail=@Mail", Con);
                cmd.Parameters.AddWithValue("@Uname", UnameTb.Text);
                cmd.Parameters.AddWithValue("@Fname", FnameTb.Text);
                cmd.Parameters.AddWithValue("@Password", PasswordTb.Text);
                cmd.Parameters.AddWithValue("@Mail", MailTb.Text);
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


        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }

        private void PasswordTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void UnameTb_Enter(object sender, EventArgs e)
        {
            if (UnameTb.Text == "UserName")
            {
                UnameTb.Text = "";
                UnameTb.ForeColor = Color.SlateGray;
            }

        }

        private void UnameTb_Leave(object sender, EventArgs e)
        {
            if (UnameTb.Text == "")
            {
                UnameTb.Text = "UserName";
                UnameTb.ForeColor = Color.SlateGray;
            }
        }

        private void FnameTb_Enter(object sender, EventArgs e)
        {
            if (FnameTb.Text == "FullName")
            {
                FnameTb.Text = "";
                FnameTb.ForeColor = Color.SlateGray;
            }
        }

        private void FnameTb_Leave(object sender, EventArgs e)
        {
            if (FnameTb.Text == "")
            {
                FnameTb.Text = "FullName";
                FnameTb.ForeColor = Color.SlateGray;
            }
        }

        private void MailTb_Enter(object sender, EventArgs e)
        {
            if (MailTb.Text == "E-mail")
            {
                MailTb.Text = "";
                MailTb.ForeColor = Color.SlateGray;
            }
        }

        private void MailTb_Leave(object sender, EventArgs e)
        {
            if (MailTb.Text == "")
            {
                MailTb.Text = "E-mail";
                MailTb.ForeColor = Color.SlateGray;
            }
        }

        private void PasswordTb_Enter(object sender, EventArgs e)
        {
            if (MailTb.Text == "Password")
            {
                MailTb.Text = "";
                MailTb.ForeColor = Color.SlateGray;
            }
        }

        private void PasswordTb_Leave(object sender, EventArgs e)
        {
            if (MailTb.Text == "")
            {
                MailTb.Text = "Password";
                MailTb.ForeColor = Color.SlateGray;
            }
        }
    }
}
