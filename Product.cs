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
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace PetFood_Project
{
    public partial class Product : Form
    { 

        private string username;
        public Product(string username)
        {
            InitializeComponent();
            this.username = username;
            label1.Text = username;
        }

        private void Dashboard_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard(username);
            ds.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Order or = new Order(username);
            or.Show();
            this.Hide();
        }

        private void btn_CRUD_Click(object sender, EventArgs e)
        {
            CRUD cRUD = new CRUD(username);
            cRUD.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Product_Load(object sender, EventArgs e)
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

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            // membuat koneksi ke database MySQL
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            // membuka koneksi ke database
            connection.Open();

            // membuat command untuk mencari data pada tabel
            MySqlCommand command = new MySqlCommand("SELECT * FROM product WHERE product_name LIKE @keyword OR product_category LIKE @keyword", connection);
            command.Parameters.AddWithValue("@keyword", "%" + guna2TextBox1.Text + "%");

            // membuat adapter dan dataset untuk menampung hasil pencarian
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataSet dataSet = new DataSet();

            // mengisi dataset dengan data dari tabel
            adapter.Fill(dataSet);

            // menampilkan hasil pencarian pada DataGridView
            datagridview1.DataSource = dataSet.Tables[0];

            // menutup koneksi ke database
            connection.Close();
        }
    }
}
