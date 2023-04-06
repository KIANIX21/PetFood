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
    public partial class receipt : Form
    {
        public receipt()
        {
            InitializeComponent();
        }

        private void lbl_user_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
        }
    }
}
