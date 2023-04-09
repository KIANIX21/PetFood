using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;
using System.Drawing.Printing;

namespace PetFood_Project
{
    public partial class receipt : Form
    {
        private string username;
        private string ordercode;
        private decimal total;

        public string Username
        {
            get { return username; }
            set { username = value; lbl_user.Text = value; }
        }
        public string OrderCode
        {
            get { return ordercode; }
            set { ordercode = value; lbl_code.Text = value; }
        }
        public decimal Total
        {
            get { return total; }
            set { total = value; lbl_subtotal.Text = value.ToString(); }
        }
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

        private void lbl_code_Click(object sender, EventArgs e)
        {

        }

        private void lbl_nama_Click(object sender, EventArgs e)
        {

        }

        private void lbl_subtotal_Click(object sender, EventArgs e)
        {

        }

        private void receipt_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_petfood;uid=root;password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT p.product_name, od.qty, od.subtotal, o.order_date FROM order_detail od JOIN product p ON od.product_code = p.product_code JOIN order_header o ON od.order_code = o.order_code WHERE od.order_code=@ordercode";
            command.Parameters.AddWithValue("@ordercode", ordercode);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            Dictionary<string, Tuple<int, decimal>> productInfo = new Dictionary<string, Tuple<int, decimal>>();
            DateTime orderDate = DateTime.MinValue;
            while (reader.Read())
            {
                string productName = reader.GetString("product_name");
                int quantity = reader.GetInt16("qty");
                decimal subtotal = reader.GetDecimal("subtotal");
                orderDate = reader.GetDateTime("order_date");

                if (!productInfo.ContainsKey(productName))
                {
                    productInfo[productName] = Tuple.Create(quantity, subtotal);
                }
                else
                {
                    var info = productInfo[productName];
                    productInfo[productName] = Tuple.Create(info.Item1 + quantity, info.Item2 + subtotal);
                }
            }
            reader.Close();
            connection.Close();

            lbl_nama.Text = "";
            lbl_qty.Text = "";
            lbl_subtotal.Text = "";
            decimal total = 0;
            foreach (var pair in productInfo)
            {
                lbl_nama.Text += pair.Key + Environment.NewLine;
                lbl_qty.Text += pair.Value.Item1 + Environment.NewLine;
                lbl_subtotal.Text += pair.Value.Item2.ToString("C0", new CultureInfo("id-ID")) + Environment.NewLine;
                total += pair.Value.Item2;
            }

            lbl_total.Text = total.ToString("C0", new CultureInfo("id-ID"));
            lbl_date.Text = orderDate.ToShortDateString();
        }

        private void btnPrintPDF_Click(object sender, EventArgs e)
        {
            // Buat document PDF baru
            var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 50, 50, 25, 25);

            // Tentukan lokasi untuk menyimpan file PDF
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF (*.pdf)|*.pdf";
            saveFileDialog1.Title = "Save PDF";
            saveFileDialog1.FileName = "Form.pdf";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Buka file stream untuk menyimpan PDF
            var fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);

            // Tambahkan konten ke document PDF
            doc.Open();
            var header = new Paragraph("Struk Pembayaran");
            header.Alignment = Element.ALIGN_CENTER;
            doc.Add(header);
            // Tambahkan konten form ke document PDF

            var alamat = new Paragraph($"{lbl_alamat.Text}");
            alamat.Alignment = Element.ALIGN_CENTER;
            doc.Add(alamat);
            doc.Add(new Paragraph($"{lbl_date.Text}"));
            doc.Add(new Paragraph($"{lbl_username.Text} {lbl_user.Text}"));
            doc.Add(new Paragraph($"{lbl_order_code.Text} {lbl_code.Text}"));
            doc.Add(new Paragraph($"{lbl_nama.Text} {lbl_qty.Text} {lbl_subtotal.Text}"));
            doc.Add(new Paragraph($"Total: {lbl_total.Text}"));


            doc.Close();
           
            // Buka file PDF dengan program default
            Process.Start(saveFileDialog1.FileName);

        }
    }
}
