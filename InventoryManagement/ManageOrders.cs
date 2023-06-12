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
using System.Data.Common;
using System.Globalization;

namespace InventoryManagement
{
    public partial class ManageOrders : Form
    {
        DataTable table;
        public ManageOrders()
        {
            InitializeComponent();
            table = new DataTable();
            table.Columns.Add("Num", typeof(int));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Qty", typeof(int));
            table.Columns.Add("UnitPrice", typeof(int));
            table.Columns.Add("TotalPrice", typeof(int));
            OrdersGV.DataSource = table;
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
        void populateproducts()
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

        void updateproduct() 
        {
            Con.Open();
            long id = Convert.ToInt64(ProductsGV.SelectedRows[0].Cells[0].Value.ToString());
            long newQty = stock - Convert.ToInt64(QtyTb.Text);
            if (newQty < 0)
                MessageBox.Show("Veprimi nuk mund te kryhet.");
            else
            {

                string query = "update ProductTbl set ProdQty = " + newQty + " where ProdId ='" + id + "'";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                Con.Close();
                populateproducts();
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int num = 0;
        int qty, uprice, totprice;
        string product;
        private void ManageOrders_Load(object sender, EventArgs e)
        {
            timerDateAndTime.Start();
            populate();
            populateproducts();
        }

        private void timerDateAndTime_Tick(object sender, EventArgs e)
        {
            lblTimeAndDate.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
        }

        private void CostumersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CustId.Text = CostumersGV.SelectedRows[0].Cells[0].Value.ToString();
            CustName.Text = CostumersGV.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void search_Enter(object sender, EventArgs e)
        {
            if (search.Text == "Kerko ketu...")
            {
                search.Text = "";
                search.ForeColor = Color.SlateGray;
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

        int flag = 0;
        int stock;
        private void ProductsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < ProductsGV.Rows.Count)
            {
                DataGridViewRow row = ProductsGV.Rows[e.RowIndex];
                product = row.Cells[1].Value.ToString();
                stock= Convert.ToInt32(row.Cells[2].Value);
                uprice = Convert.ToInt32(row.Cells[3].Value);
                flag = 1;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int sum = 0;
            if (QtyTb.Text == "")
                MessageBox.Show("Shkruani sasise e produktit.");
            else if (flag == 0)
                MessageBox.Show("Zgjidhni produktin.");
            else if (Convert.ToInt32(QtyTb.Text) > stock)
                MessageBox.Show("Ska produkt mjaftueshem.");
            else
            {
                num = num + 1;
                qty = Convert.ToInt32(QtyTb.Text);
                totprice = qty * uprice;
                table.Rows.Add(num, product, qty, uprice, totprice);
                OrdersGV.DataSource = table;
                flag = 0;

                updateproduct();
            }

            // Calculate the sum of total prices
            foreach (DataRow row in table.Rows)
            {
                sum += Convert.ToInt32(row["TotalPrice"]);
            }
            TotAmount.Text = sum.ToString() + " LEK";
        }




        private void button2_Click(object sender, EventArgs e)
        {
            if (OrderIdTb.Text == "" || CustId.Text == "" || CustName.Text == "" || TotAmount.Text == "")
            {
                MessageBox.Show("Plotesoni te gjitha vendet bosh.");
            }
            else
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO OrderTbl (OrderId, CustId, CustName, OrderDate, TotalAmt) VALUES (@OrderId, @CustId, @CustName, @OrderDate, @TotalAmt)", Con);
                cmd.Parameters.AddWithValue("@OrderId", Convert.ToInt32(OrderIdTb.Text));
                cmd.Parameters.AddWithValue("@CustId", CustId.Text);
                cmd.Parameters.AddWithValue("@CustName", CustName.Text);
                cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now); // Example: using the current date and time

                // Convert the string value to decimal and provide it as the value for the "@TotalAmt" parameter
                decimal totalAmt = 0;
                decimal.TryParse(TotAmount.Text.Trim().Split(' ')[0], NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out totalAmt);
                cmd.Parameters.AddWithValue("@TotalAmt", totalAmt);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Porosia u krye me sukses");
                Con.Close();
            }
        }




        private void button3_Click(object sender, EventArgs e)
        {
            ViewOrders view= new ViewOrders();  
            view.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            label4.FlatStyle = FlatStyle.Standard;
            label4.BackColor = Color.Transparent;
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            if (search.Text.Length == 0)
            {
                return;
            }

            string input = search.Text;
            string connstr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Desktop\projekt c#\InventoryManagement\InventoryManagement\Marketdb.mdf;Integrated Security=True"; 
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
            DataTable dt = new DataTable(); 
            da.Fill(dt);
            Con.Close();
            ProductsGV.DataSource = dt;
            ProductsGV.Refresh();

        }

        private void refresh_Click(object sender, EventArgs e)
        {
            populateproducts();
            search.Text = "";

        }
    }
}
