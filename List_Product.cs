using Guna.UI2.WinForms;
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

namespace PetFood_Project
{
    public partial class List_Product : Form
    {
        private string username;

        public List_Product(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
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

        private void List_Product_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM product";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Menambahkan kolom checkbox
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "Select";
            checkBoxColumn.Width = 50;
            checkBoxColumn.ReadOnly = false;
            datagridview1.Columns.Insert(0, checkBoxColumn);

            datagridview1.DataSource = dataTable;
            datagridview1.Columns[1].HeaderText = "Code";
            datagridview1.Columns[2].HeaderText = "Name";
            datagridview1.Columns[3].HeaderText = "Category";
            datagridview1.Columns[4].HeaderText = "Stock";
            datagridview1.Columns[5].HeaderText = "Price";
            datagridview1.Columns[6].HeaderText = "Usercode";

            connection.Close();
        }

        private void btn_tambahpesanan_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            string newOrderCode;

            // Mendapatkan kode order terbaru dari database
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

            // Menambahkan detail pesanan ke database
            foreach (DataGridViewRow row in datagridview1.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    // Ambil data dari baris ini
                    string productCode = row.Cells[1].Value.ToString();
                    decimal price = Convert.ToDecimal(row.Cells[5].Value);
                    int quantity = 1; // Set jumlah ke 1

                    // Mengecek apakah item pesanan sudah ada di database
                    bool itemExists = false;
                    int existingQty = 0;
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "SELECT qty FROM order_detail WHERE order_code = @order_code AND product_code = @product_code";
                            cmd.Parameters.AddWithValue("@order_code", newOrderCode);
                            cmd.Parameters.AddWithValue("@product_code", productCode);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    itemExists = true;
                                    existingQty = Convert.ToInt32(reader["qty"]);
                                }
                            }
                        }
                        conn.Close();
                    }

                    // Jika item pesanan sudah ada, tambahkan kuantitasnya
                    if (itemExists)
                    {
                        quantity += existingQty;
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = "UPDATE order_detail SET qty = @quantity, subtotal = @subtotal WHERE order_code = @order_code AND product_code = @product_code";
                                cmd.Parameters.AddWithValue("@quantity", quantity);
                                cmd.Parameters.AddWithValue("@subtotal", price * quantity);
                                cmd.Parameters.AddWithValue("@order_code", newOrderCode);
                                cmd.Parameters.AddWithValue("@product_code", productCode);
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();
                        }
                    }
                    // Jika item pesanan belum ada, tambahkan item baru ke database
                    else
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = "INSERT INTO order_detail (order_code, product_code, qty, subtotal) VALUES (@order_code, @product_code, @quantity, @subtotal)";
                                cmd.Parameters.AddWithValue("@order_code", newOrderCode);
                                cmd.Parameters.AddWithValue("@product_code", productCode);
                                cmd.Parameters.AddWithValue("@quantity", quantity);
                                cmd.Parameters.AddWithValue("@subtotal", price * quantity);
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();
                        }
                    }
                    MessageBox.Show("Pesanan berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }
}