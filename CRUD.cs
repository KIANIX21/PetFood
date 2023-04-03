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
    public partial class CRUD : Form
    {
        private string username;
        public CRUD(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void CRUD_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM product";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            datagridview1.DataSource = dataTable;
            datagridview1.Columns[0].HeaderText = "Code";
            datagridview1.Columns[1].HeaderText = "Name";
            datagridview1.Columns[2].HeaderText = "Category";
            datagridview1.Columns[3].HeaderText = "Stock";
            datagridview1.Columns[4].HeaderText = "Price";
            datagridview1.Columns[5].HeaderText = "Usercode";

            connection.Close();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            Product pr = new Product(username);
            pr.Show();
            this.Hide();
        }

        private void datagridview1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = datagridview1.Rows[e.RowIndex];

                // Mengambil nilai dari setiap kolom di dalam row
                string nilaiKolom2 = row.Cells[1].Value.ToString();
                string nilaiKolom3 = row.Cells[2].Value.ToString();
                string nilaiKolom4 = row.Cells[3].Value.ToString();
                string nilaiKolom5 = row.Cells[4].Value.ToString();

                // Menampilkan nilai pada TextBox
                txt_name.Text = nilaiKolom2;
                txt_category.Text = nilaiKolom3;
                txt_stock.Text = nilaiKolom4;
                txt_price.Text = nilaiKolom5;
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string usercode = null;
            MySqlCommand cmd = new MySqlCommand("SELECT user_code FROM users WHERE user_name=@username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                usercode = reader.GetString(0);
            }
            reader.Close();

            // Cek apakah field sudah diisi semua
            if (txt_name.Text == "" || txt_category.Text == "" || txt_stock.Text == "" || txt_price.Text == "")
            {
                MessageBox.Show("Harap isi semua field!");
                return;
            }

            // Update data pada tabel
            string updateQuery = "UPDATE product SET product_category = @category, product_stock = @stock, product_price = @price, user_code = @usercode WHERE product_name = @nama";

            try
            {
                // Lakukan koneksi ke database dan eksekusi query
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nama", txt_name.Text);
                        command.Parameters.AddWithValue("@category", txt_category.Text);
                        command.Parameters.AddWithValue("@stock", txt_stock.Text);
                        command.Parameters.AddWithValue("@price", txt_price.Text);
                        command.Parameters.AddWithValue("@usercode", usercode);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    string selectQuery = "SELECT * FROM product";
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            datagridview1.DataSource = dataTable;
                        }
                    }

                    MessageBox.Show("Data berhasil diupdate!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btn_cr_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            // Ambil id dari pengguna yang login
            string idPengguna = getIdPengguna();

            // Cek apakah field sudah diisi semua
            if (txt_name.Text == "" || txt_category.Text == "" || txt_stock.Text == "" || txt_price.Text == "")
            {
                MessageBox.Show("Harap isi semua field!");
                return;
            }

            // Simpan data ke database
            string query = "INSERT INTO product (product_name, product_category, product_stock, product_price, user_code) VALUES (@nama, @category, @stock, @price, @idPengguna)";

            try
            {
                // lakukan koneksi ke database dan eksekusi query
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", txt_name.Text);
                        cmd.Parameters.AddWithValue("@category", txt_category.Text);
                        cmd.Parameters.AddWithValue("@stock", txt_stock.Text);
                        cmd.Parameters.AddWithValue("@price", txt_price.Text);
                        cmd.Parameters.AddWithValue("@idPengguna", idPengguna);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    string selectQuery = "SELECT * FROM product";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connectionString))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        datagridview1.DataSource = dataTable;
                    }
                }

                MessageBox.Show("Data berhasil disimpan!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private string getIdPengguna()
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            // Contoh pengambilan id pengguna dari database
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "delete user_code FROM users WHERE user_name = @username";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {

            }
        }
    }
}
