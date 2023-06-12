using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.Data.SqlClient;

namespace InventoryManagement
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Desktop\projekt c#\InventoryManagement\InventoryManagement\Marketdb.mdf;Integrated Security=True");
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = @"C:\Users\User\Desktop\projekt c#\foto\Untitled video - Made with Clipchamp (3).mp4";
            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.settings.autoStart = true;
            axWindowsMediaPlayer1.settings.mute = true;
            axWindowsMediaPlayer1.settings.setMode("loop", true);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                UpasswordTb.UseSystemPasswordChar = true;
            else
                UpasswordTb.UseSystemPasswordChar = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            UmailTb.Text = "";
            UpasswordTb.Text = "";
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (UmailTb.Text == "E-mail Address")
            {
                UmailTb.Text = "";
                UmailTb.ForeColor = Color.White;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (UmailTb.Text == "")
            {
                UmailTb.Text = "E-mail Address";
                UmailTb.ForeColor = Color.White;
            }

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (UpasswordTb.Text == "Password")
            {
                UpasswordTb.Text = "";
                UpasswordTb.UseSystemPasswordChar = false;
                UpasswordTb.ForeColor = Color.White;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (UpasswordTb.Text == "")
            {
                UpasswordTb.Text = "Password";
                UpasswordTb.UseSystemPasswordChar= true;
                UpasswordTb.ForeColor = Color.White;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM Usertbl WHERE Umail = @Umail AND Upassword = @Upassword", Con);
            sda.SelectCommand.Parameters.AddWithValue("@Umail", UmailTb.Text);
            sda.SelectCommand.Parameters.AddWithValue("@Upassword", UpasswordTb.Text);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                HomePage home = new HomePage();
                home.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email ose password i gabuar!");
            }
            Con.Close();
        }

    }
}
