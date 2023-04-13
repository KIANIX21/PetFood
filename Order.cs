using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PetFood_Project
{
    public partial class Order : Form
    {
        private string username;
        private string ordercode;
        private decimal total; // tambahkan variabel total
        public string OrderCode // Property untuk menampung nilai ordercode
        {
            get { return ordercode; }
            set { ordercode = value; lbl_code.Text = ordercode; }
        }
        public decimal Total { get { return total; } }

        public Order(string username)
        {
            InitializeComponent();
            this.username = username;
            label1.Text = username;
            this.lbl_harga.Text = total.ToString();
        }

        private void Dashboard_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard(username);
            ds.Show();
            this.Hide();
        }

        private void btn_product_Click(object sender, EventArgs e)
        {
            Product pr = new Product(username);
            pr.Show();
            this.Hide();
        }

        private void btn_LogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Are you sure you want to logout?!", "Warning",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                Login lg = new Login();
                lg.Show();
                this.Hide();
            }
        }


        private void btn_checkout_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtprice.Text))
            {
                MessageBox.Show("Please enter the payment amount.");
                return;
            }
            decimal total = decimal.Parse(lbl_harga.Text);
            decimal pay = decimal.Parse(txtprice.Text);
            if (pay < total)
            {
                MessageBox.Show("Your Money Is Less!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            receipt rp = new receipt();
            rp.Username = username;
            rp.OrderCode = ordercode;
            rp.Pay = decimal.Parse(txtprice.Text); // Inisialisasi nilai Pay
            rp.ShowDialog();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            List_Product or = new List_Product(username);
            or.ShowDialog();
            this.Dispose();
        }

        private void Order_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT product_code, qty, subtotal FROM order_detail WHERE order_code = @order_code";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@order_code", ordercode);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            table_order.DataSource = dataTable;
            table_order.Columns[0].HeaderText = "Product_Code";
            table_order.Columns[1].HeaderText = "Qty";
            table_order.Columns[2].HeaderText = "Subtotal";

            total = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                total += Convert.ToDecimal(row["subtotal"]);
            }
            lbl_harga.Text = total.ToString();

            connection.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Confirm_Order co = new Confirm_Order(username, ordercode, total);
            co.username = username;
            co.ordercode = ordercode;
            co.total = total;
            co.ShowDialog();
            this.Hide();
        }

        private void btn_hasil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtprice.Text))
            {
                MessageBox.Show("Please enter the payment amount.");
                return;
            }
            decimal total = decimal.Parse(lbl_harga.Text);
            decimal pay = decimal.Parse(txtprice.Text);
            if (pay < total)
            {
                MessageBox.Show("Your Money Is Less!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                decimal hasil = pay - total;
                lbl_hasil.Text = hasil.ToString("C0", new CultureInfo("id-ID"));
            }
        }

        private void lbl_hasil_Click(object sender, EventArgs e)
        {

        }
    }
}
