using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetFood_Project
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";

            // Check if the username and password fields are empty
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(this, "Please enter username and password!!", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                // Set focus to the first empty field
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    txtUsername.Focus();
                }
                else if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    txtPassword.Focus();
                }

                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create a MySQL query to select the user with the specified username and hashed password
                string query = "SELECT * FROM users WHERE user_name = @username AND user_password = @password";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters to the query
                    command.Parameters.AddWithValue("@username", txtUsername.Text);
                    command.Parameters.AddWithValue("@password", txtPassword.Text);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Login successful, get the username and open the next form
                            reader.Read();
                            string username = reader["user_name"].ToString();

                            // Create the next form with the username as parameter and show it
                            Dashboard formDashboard = new Dashboard(username);
                            formDashboard.Show();

                            // Hide the current form
                            this.Hide();
                        }
                        else
                        {
                            // Login failed, show an error message
                            MessageBox.Show(this, "Invalid username and password!!", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
            }
        }
    }
}
