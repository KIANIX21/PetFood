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
        public Product()
        {
            InitializeComponent();
        }

        private void Dashboard_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
            ds.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
        //    order or = new order();
        //    or.show();
        //    this.Hide();
        }

        private void btn_CRUD_Click(object sender, EventArgs e)
        {
            CRUD cRUD = new CRUD();
            cRUD.Show();
            this.Hide();
        }
    }
}
