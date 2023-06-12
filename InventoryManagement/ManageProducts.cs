using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;

namespace InventoryManagement
{
    public partial class ManageProducts : Form
    {
        public ManageProducts()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Desktop\projekt c#\InventoryManagement\InventoryManagement\Marketdb.mdf;Integrated Security=True");

        void populate()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from ProductTbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                ProductsGV.DataSource = ds.Tables[0];
                Con.Close();


            }
            catch
            {

            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
        }

        private void timerDateAndTime_Tick(object sender, EventArgs e)
        {
            lblTimeAndDate.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
        }

        private void ManageProducts_Load(object sender, EventArgs e)
        {
            timerDateAndTime.Start();
            populate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ProductTbl VALUES (@ProdId, @ProdName, @ProdQty, @ProdPrice)", Con);
                cmd.Parameters.AddWithValue("@ProdId", PidTb.Text);
                cmd.Parameters.AddWithValue("@ProdName", PnameTb.Text);
                cmd.Parameters.AddWithValue("@ProdQty", PqtyTb.Text);
                cmd.Parameters.AddWithValue("@ProdPrice", PpriceTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product added!");
                Con.Close();
                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE ProductTbl SET ProdName=@ProdName, ProdPrice=@ProdPrice, ProdQty=@ProdQty where ProdId = @ProdId", Con);
                cmd.Parameters.AddWithValue("@ProdId", PidTb.Text);
                cmd.Parameters.AddWithValue("@ProdPrice", PpriceTb.Text);
                cmd.Parameters.AddWithValue("@ProdQty", PqtyTb.Text);
                cmd.Parameters.AddWithValue("@ProdName", PnameTb.Text);
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


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (PnameTb.Text == "")
            {
                MessageBox.Show("Shkruani emrin.");
            }
            else
            {
                Con.Open();
                string myquery = "DELETE FROM ProductTbl WHERE ProdName=@ProdName";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.Parameters.AddWithValue("@ProdName", PnameTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produkti u fshi.");
                Con.Close();
                populate();
            }
        }


        private void ProductsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PidTb.Text = ProductsGV.SelectedRows[0].Cells[0].Value.ToString();
            PnameTb.Text = ProductsGV.SelectedRows[0].Cells[1].Value.ToString();
            PqtyTb.Text = ProductsGV.SelectedRows[0].Cells[2].Value.ToString();
            PpriceTb.Text = ProductsGV.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void PidTb_Enter(object sender, EventArgs e)
        {
            if (PidTb.Text == "Id Produktit")
            {
                PidTb.Text = "";
                PidTb.ForeColor = Color.SlateGray;
            }
        }

        private void PidTb_Leave(object sender, EventArgs e)
        {
            if (PidTb.Text == "")
            {
                PidTb.Text = "Id Produktit";
                PidTb.ForeColor = Color.SlateGray;
            }
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            if (search.Text.Length == 0)
            {
                return;
            }

            string input = search.Text;
            string connstr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Desktop\projekt c#\InventoryManagement\InventoryManagement\Marketdb.mdf;Integrated Security=True"; // Update the connection string here
            SqlConnection Con = new SqlConnection(connstr);

            Con.Open();
            SqlCommand cmd;

            if (Regex.IsMatch(input, " ^[0-9]+$"))
            {
                int ninput;
                bool result = int.TryParse(input, out ninput);
                if (result)
                {
                    string query = "SELECT * FROM ProductTbl where ProdName LIKE @ProdName or ProdId LIKE @ProdId";
                    cmd = new SqlCommand(query, Con);
                    cmd.Parameters.Add(new SqlParameter("ProdName", ninput + "%"));
                    cmd.Parameters.Add(new SqlParameter("ProdId", ninput));
                }
                else
                {
                    string query = "SELECT * FROM ProductTbl where ProdId=@ProdId";
                    cmd = new SqlCommand(query, Con);
                    cmd.Parameters.Add(new SqlParameter("ProdId", ninput));
                }
            }
            else
            {
                string query = "SELECT * FROM ProductTbl where ProdName LIKE @ProdName or ProdId LIKE @ProdId";
                cmd = new SqlCommand(query, Con);
                cmd.Parameters.Add(new SqlParameter("ProdName", input + "%"));
                cmd.Parameters.Add(new SqlParameter("ProdId", input));
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable(); // Instantiate the DataTable
            da.Fill(dt);
            Con.Close();
            ProductsGV.DataSource = dt;
            ProductsGV.Refresh();
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            populate();
            search.Text = "";
        }

        private void search_Enter(object sender, EventArgs e)
        {
            if (search.Text == "Kerko ketu...")
            {
                search.Text = "";
                search.ForeColor = Color.LightSlateGray;
            }

        }

        private void search_Leave(object sender, EventArgs e)
        {
            if (search.Text == "")
            {
                search.Text = "Kerko ketu...";
                search.ForeColor = Color.SlateGray;
            }
        }

        private void PnameTb_Enter(object sender, EventArgs e)
        {
            if (PnameTb.Text == "Emri")
            {
                PnameTb.Text = "";
                PnameTb.ForeColor = Color.LightSlateGray;
            }

        }

        private void PnameTb_Leave(object sender, EventArgs e)
        {
            if (PnameTb.Text == "")
            {
                PnameTb.Text = "Emri";
                PnameTb.ForeColor = Color.SlateGray;
            }
        }

        private void PqtyTb_Enter(object sender, EventArgs e)
        {
            if (PqtyTb.Text == "Sasia")
            {
                PqtyTb.Text = "";
                PqtyTb.ForeColor = Color.SlateGray;
            }
        }

        private void PqtyTb_Leave(object sender, EventArgs e)
        {
            if (PqtyTb.Text == "")
            {
                PqtyTb.Text = "Sasia";
                PqtyTb.ForeColor = Color.SlateGray;
            }
        }

        private void PpriceTb_Enter(object sender, EventArgs e)
        {
            if (PpriceTb.Text == "Çmimi")
            {
                PpriceTb.Text = "";
                PpriceTb.ForeColor = Color.SlateGray;
            }
        }

        private void PpriceTb_Leave(object sender, EventArgs e)
        {
            if (PpriceTb.Text == "")
            {
                PpriceTb.Text = "Çmimi";
                PpriceTb.ForeColor = Color.SlateGray;
            }
        }

        private void btnPastro_Click(object sender, EventArgs e)
        {
            PidTb.Text = string.Empty;
            PnameTb.Text = string.Empty;
            PpriceTb.Text = string.Empty;
            PqtyTb.Text = string.Empty;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomePage home=new HomePage();   
            home.Show();
            this.Hide();
        }
    }


    }

