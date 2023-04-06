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
        private string username;
        private int usercode;
        public Confirm_Order()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            Order or = new Order(username,usercode);
            or.Show();
            this.Hide();
        }
    }
}
