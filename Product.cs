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
            CRUD cRUD = new CRUD();
            cRUD.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }
    }
}
