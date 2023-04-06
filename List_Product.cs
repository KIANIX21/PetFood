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
        private int usercode;
        public List_Product()
        {
            InitializeComponent();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
