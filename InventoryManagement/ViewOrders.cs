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
    public partial class ViewOrders : Form
    {
        public ViewOrders()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Desktop\projekt c#\InventoryManagement\InventoryManagement\Marketdb.mdf;Integrated Security=True");
        void populateorders()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from OrderTbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                OrdersGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();    
        }

        private void ViewOrders_Load(object sender, EventArgs e)
        {
            populateorders();
        }

        private void OrdersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(printPreviewDialog1.ShowDialog()==DialogResult.OK)
            {
                printDocument1.Print();
            }
            

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Porosite e kryera", new Font("Century", 25, FontStyle.Bold), Brushes.DarkBlue, new Point(230));
            e.Graphics.DrawString("Id Porosise:"+OrdersGV.SelectedRows[0].Cells[0].Value.ToString(), new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80,100));
            e.Graphics.DrawString("Id Klientit:" + OrdersGV.SelectedRows[0].Cells[1].Value.ToString(), new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80, 135));
            e.Graphics.DrawString("Emri Klientit:" + OrdersGV.SelectedRows[0].Cells[2].Value.ToString(), new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80, 165));
            e.Graphics.DrawString("Data e porosise:" + OrdersGV.SelectedRows[0].Cells[3].Value.ToString(), new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80, 195));
        }
    }
}
