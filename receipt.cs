﻿using MySql.Data.MySqlClient;
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
using iTextSharp.text.pdf.draw;

namespace PetFood_Project
{
    public partial class receipt : Form
    {
        private string username;
        private string ordercode;
        private decimal pay;
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
        public decimal Pay
        {
            get { return pay; }
            set { pay = value; lblpay.Text = value.ToString("C0", new CultureInfo("id-ID")); }
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
            decimal change = pay - total;
            lblchange.Text = change.ToString("C0", new CultureInfo("id-ID"));
            lbl_date.Text = orderDate.ToShortDateString();
        }

        private void btnPrintPDF_Click(object sender, EventArgs e)
        {
            // Buat document PDF baru
            var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A5, 50, 50, 25, 25);

            // Tentukan lokasi untuk menyimpan file PDF
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF (*.pdf)|*.pdf";
            saveFileDialog1.Title = "Save PDF";
            saveFileDialog1.FileName = "Receipt.pdf";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Buka file stream untuk menyimpan PDF
            var fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);

            // Tambahkan konten ke document PDF
            doc.Open();
            var header = new Paragraph("Receipt of Payment");
            header.Alignment = Element.ALIGN_CENTER;
            doc.Add(header);

            var alamat = new Paragraph($"{lbl_alamat.Text}");
            alamat.Alignment = Element.ALIGN_CENTER;
            doc.Add(alamat);
            doc.Add(new Paragraph($"Tanggal :{lbl_date.Text}"));
            doc.Add(new Paragraph($"Username : {lbl_user.Text}"));
            doc.Add(new Paragraph($"Order Code : {lbl_code.Text}"));
            doc.Add(new Paragraph($"--------------------------------------------------------------------------------"));
            var table = new PdfPTable(3);
            table.WidthPercentage = 100;
            table.DefaultCell.BorderWidth = 0;

            table.AddCell("Name Product");
            table.AddCell("Quantity");
            table.AddCell("Subtotal");

            List<string> names = lbl_nama.Text.Split(',').ToList();
            List<string> quantities = lbl_qty.Text.Split(',').ToList();
            List<string> subtotals = lbl_subtotal.Text.Split(',').ToList();

            for (int i = 0; i < names.Count; i++)
            {
                table.AddCell(names[i]);
                table.AddCell(quantities[i]);
                table.AddCell(subtotals[i]);
            }
            doc.Add(table);

            var totalParagraph = new Paragraph();
            totalParagraph.SpacingBefore = 300f;

            var totalLabel = new Chunk("Total :");

            var totalValueChunk = new Chunk(lbl_total.Text);

            totalParagraph.Add(totalLabel);
            totalParagraph.Add(new Chunk(new VerticalPositionMark())); // menambahkan penanda posisi vertikal untuk memisahkan teks kiri dan kanan
            totalParagraph.Add(totalValueChunk);
            totalParagraph.Alignment = Element.ALIGN_RIGHT; // atur posisi paragraf ke kanan

            doc.Add(totalParagraph);

            var payParagraph = new Paragraph();

            var payLabel = new Chunk("Pay :");

            var payValueChunk = new Chunk(lblpay.Text);
            payValueChunk.SetUnderline(0.1f, -2f);

            payParagraph.Add(payLabel);
            payParagraph.Add(new Chunk(new VerticalPositionMark())); // menambahkan penanda posisi vertikal untuk memisahkan teks kiri dan kanan
            payParagraph.Add(payValueChunk);
            payParagraph.Alignment = Element.ALIGN_RIGHT; // atur posisi paragraf ke kanan

            doc.Add(payParagraph);

            var changeParagraph = new Paragraph();

            var changeLabel = new Chunk("Change : ");

            var changeValueChunk = new Chunk(lblchange.Text);

            changeParagraph.Add(changeLabel);
            changeParagraph.Add(new Chunk(new VerticalPositionMark())); // menambahkan penanda posisi vertikal untuk memisahkan teks kiri dan kanan
            changeParagraph.Add(changeValueChunk);
            changeParagraph.Alignment = Element.ALIGN_RIGHT; // atur posisi paragraf ke kanan

            doc.Add(changeParagraph);
            


            doc.Close();
            // Buka file PDF dengan program default
            Process.Start(saveFileDialog1.FileName);
        }

        private void lbl_check_Click(object sender, EventArgs e)
        {

        }
    }
}
