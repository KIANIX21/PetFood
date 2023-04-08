﻿using System;
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
        public string username;
        private string ordercode;
        private decimal total;

        public Dashboard(string username)
        {
            InitializeComponent();
            // Set the username as instance variable
            this.username = username;

            
            label2.Text = username;
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btn_product_Click(object sender, EventArgs e)
        {
            Product pr = new Product(username);
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
            Confirm_Order co = new Confirm_Order(username, ordercode, total);
            co.ShowDialog();
            this.Dispose();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
