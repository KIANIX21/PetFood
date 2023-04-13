using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PetFood_Project
{
    public partial class Confirm_Order : Form
    {
        public string username;
        public string ordercode;
        public decimal total;
        public Confirm_Order(string username, string ordercode, decimal total)
        {
            InitializeComponent();
            this.username = username;
            this.ordercode = ordercode;
            string idPengguna = getIdPengguna();
            this.total = total; // tambahkan inisialisasi nilai total
            this.txt_user.Text = idPengguna;
            txt_total.Text = total.ToString();
        }

        private string getIdPengguna()
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            string query = "SELECT user_code FROM users WHERE user_name = @username";
            string idPengguna;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    conn.Open();
                    idPengguna = cmd.ExecuteScalar().ToString();
                    conn.Close();
                }
            }
            return idPengguna;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO order_header (order_date, order_total, user_code) VALUES (@orderdate, @ordertotal, @usercode)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@usercode", txt_user.Text);
                        DateTime orderDate = datetime.Value;
                        if (orderDate > DateTime.Today)
                        {
                            MessageBox.Show("Order date cannot be in the future.");
                            return;
                        }
                        else if (orderDate < DateTime.Today)
                        {
                            MessageBox.Show("Order date cannot be before today.");
                            return;
                        }
                        cmd.Parameters.AddWithValue("@orderdate", orderDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@ordertotal", txt_total.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Order confirmed!");
                        conn.Close();
                    }
                }
                string newOrderCode;
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "SELECT MAX(order_code) FROM order_header";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        newOrderCode = cmd.ExecuteScalar().ToString();
                        conn.Close();
                    }
                }
                Order or = new Order(username);
                or.OrderCode = newOrderCode; // Menetapkan nilai ordercode ke Order form
                or.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
    }
}
