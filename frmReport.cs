using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace CSLab09_LINQ_Report
{
    public class frmReport : Form
    {
        private readonly int maHD;
        private readonly DateTime ngayLap;
        private readonly string khachHang;
        private readonly List<HangChonMua> ds;
        private readonly double tongTien;
        private readonly PrintDocument printDocument = new PrintDocument();
        private int pageLine;

        public frmReport(int id, DateTime date, string customer, List<HangChonMua> items, double total)
        {
            maHD = id; ngayLap = date; khachHang = customer; ds = items; tongTien = total;
            Text = "Hóa đơn bán hàng - xem trước/in";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(760, 600);
            printDocument.PrintPage += PrintDocument_PrintPage;
            var preview = new PrintPreviewControl { Dock = DockStyle.Fill, Document = printDocument, Zoom = 1.0 };
            Controls.Add(preview);
            var panel = new Panel { Dock = DockStyle.Bottom, Height = 48 };
            var btnPrint = new Button { Text = "In hóa đơn", Width = 120, Height = 32, Left = 15, Top = 8 };
            btnPrint.Click += (s, e) => { using (var dialog = new PrintDialog { Document = printDocument }) if (dialog.ShowDialog() == DialogResult.OK) printDocument.Print(); };
            var btnClose = new Button { Text = "Đóng", Width = 100, Height = 32, Left = 145, Top = 8 };
            btnClose.Click += (s, e) => Close();
            panel.Controls.Add(btnPrint); panel.Controls.Add(btnClose); Controls.Add(panel);
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            float x = e.MarginBounds.Left, y = e.MarginBounds.Top;
            using (var titleFont = new Font("Arial", 18, FontStyle.Bold))
            using (var bold = new Font("Arial", 10, FontStyle.Bold))
            using (var normal = new Font("Arial", 10))
            using (var pen = new Pen(Color.Black))
            {
                e.Graphics.DrawString("HÓA ĐƠN BÁN HÀNG", titleFont, Brushes.Black, x + 150, y);
                y += 45;
                e.Graphics.DrawString("Mã hóa đơn: " + maHD, normal, Brushes.Black, x, y); y += 22;
                e.Graphics.DrawString("Ngày lập: " + ngayLap.ToString("dd/MM/yyyy HH:mm"), normal, Brushes.Black, x, y); y += 22;
                e.Graphics.DrawString("Khách hàng: " + khachHang, normal, Brushes.Black, x, y); y += 32;
                e.Graphics.DrawLine(pen, x, y, x + e.MarginBounds.Width, y); y += 10;
                e.Graphics.DrawString("Mã hàng", bold, Brushes.Black, x, y);
                e.Graphics.DrawString("Tên hàng hóa", bold, Brushes.Black, x + 85, y);
                e.Graphics.DrawString("Số lượng", bold, Brushes.Black, x + 310, y);
                e.Graphics.DrawString("Đơn giá", bold, Brushes.Black, x + 405, y);
                e.Graphics.DrawString("Thành tiền", bold, Brushes.Black, x + 515, y);
                y += 25;
                foreach (var item in ds)
                {
                    e.Graphics.DrawString(item.MaHH.ToString(), normal, Brushes.Black, x, y);
                    e.Graphics.DrawString(item.TenHH, normal, Brushes.Black, x + 85, y);
                    e.Graphics.DrawString(item.SoLuong.ToString(), normal, Brushes.Black, x + 310, y);
                    e.Graphics.DrawString(item.DonGia.ToString("#,##0.##"), normal, Brushes.Black, x + 405, y);
                    e.Graphics.DrawString(item.ThanhTien.ToString("#,##0.##"), normal, Brushes.Black, x + 515, y);
                    y += 24;
                }
                y += 12;
                e.Graphics.DrawLine(pen, x, y, x + e.MarginBounds.Width, y); y += 15;
                e.Graphics.DrawString("TỔNG TIỀN: " + tongTien.ToString("#,##0.##") + " đ", titleFont, Brushes.Black, x + 300, y);
            }
            e.HasMorePages = false;
        }
    }
}
