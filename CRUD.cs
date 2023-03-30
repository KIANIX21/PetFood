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
        public CRUD()
        {
            InitializeComponent();
        }

        private void CRUD_Load(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            Product pr = new Product(username);
            pr.Show();
            this.Hide();
        }
    }
}
