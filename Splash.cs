using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace PetFood_Project
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }
        int startPoint = 0;
        private void Splash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            startPoint += 2;
            guna2ProgressBar1.Value = startPoint;
            if (guna2ProgressBar1.Value == 100)
            {
                guna2ProgressBar1.Value = 0;
                timer1.Stop();

                this.Hide();
                Login login = new Login();
                login.Show();
            }
        }

        private void guna2ProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
