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
    public partial class Order : Form
    {
        private string username;
        private int usercode;
        public Order(string username, int user_code)
        {
            InitializeComponent();
            this.username = username;
            this.usercode = user_code;
            label1.Text = username;
            label2.Text = user_code.ToString();
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Hide();
        }

        private void btn_CRUD_Click(object sender, EventArgs e)
        {
            Confirm_Order co = new Confirm_Order();
            co.ShowDialog();
            
        }

        private void btn_checkout_Click(object sender, EventArgs e)
        {
            receipt rp = new receipt();
            rp.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            List_Product or = new List_Product();
            or.Show();
            this.Hide();
        }

        private void guna2Shapes1_Click(object sender, EventArgs e)
        {

        }

        private void table_order_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Shapes2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lbl_order_Click(object sender, EventArgs e)
        {

        }

        private void lbl_harga_Click(object sender, EventArgs e)
        {

        }

        private void lbl_total_Click(object sender, EventArgs e)
        {

        }
    }
}
