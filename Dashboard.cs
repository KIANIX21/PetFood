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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        //•
        private void Dashboard_Load(object sender, EventArgs e)
        {
            label1.Parent = guna2PictureBox1;
            label1.BackColor = Color.Transparent;
            label2.Parent = guna2PictureBox1;
            label2.BackColor = Color.Transparent;
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btn_product_Click(object sender, EventArgs e)
        {
            Product pr = new Product();
            pr.Show();
            this.Hide();
        }

        private void btn_LogOut_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Hide();
        }

        private void btn_Order_Click(object sender, EventArgs e)
        {
            //Order or = new Order();
            //or.Show();
            //this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
        }
    }
}
