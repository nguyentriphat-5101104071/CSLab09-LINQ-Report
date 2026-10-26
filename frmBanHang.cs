using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace CSLab09_LINQ_Report
{
    public class frmBanHang : Form
    {
        private readonly QLBHDataContext db = new QLBHDataContext();
        private readonly List<HangChonMua> dsmua = new List<HangChonMua>();
        private TextBox txtKhachHang;
        private ComboBox cboHangHoa;
        private NumericUpDown nudSoLuong;
        private DataGridView dgvHangHoa;
        private Label lblTongTien;

        public frmBanHang()
        {
            InitializeForm();
            Load += frmBanHang_Load;
        }

        private void InitializeForm()
        {
            Text = "Quản lý bán hàng (LINQ)";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(900, 590);
            MinimumSize = new Size(780, 500);
            Font = new Font("Segoe UI", 10);
            Controls.Add(new Label { Text = "Khách hàng", Location = new Point(25, 28), AutoSize = true });
            txtKhachHang = new TextBox { Location = new Point(130, 25), Width = 360 };
            Controls.Add(txtKhachHang);
            var btnLap = new Button { Text = "Lập hóa đơn", Location = new Point(650, 22), Width = 180, Height = 34 };
            btnLap.Click += btnLapHoaDon_Click;
            Controls.Add(btnLap);

            Controls.Add(new Label { Text = "Hàng hóa", Location = new Point(25, 82), AutoSize = true });
            cboHangHoa = new ComboBox { Location = new Point(130, 78), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cboHangHoa);
            Controls.Add(new Label { Text = "Số lượng", Location = new Point(450, 82), AutoSize = true });
            nudSoLuong = new NumericUpDown { Location = new Point(530, 78), Width = 90, Minimum = 1, Maximum = 9999, Increment = 1, Value = 1 };
            Controls.Add(nudSoLuong);
            var btnThem = new Button { Text = "Thêm", Location = new Point(650, 75), Width = 180, Height = 34 };
            btnThem.Click += btnThem_Click;
            Controls.Add(btnThem);

            dgvHangHoa = new DataGridView {
                Location = new Point(25, 135), Size = new Size(805, 315), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true, AllowUserToAddRows = false, AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowHeadersVisible = false
            };
            Controls.Add(dgvHangHoa);
            Controls.Add(new Label { Text = "Tổng tiền:", Location = new Point(430, 475), AutoSize = true, Anchor = AnchorStyles.Bottom | AnchorStyles.Right });
            lblTongTien = new Label { Text = "0 đ", Location = new Point(535, 472), AutoSize = true, ForeColor = Color.Firebrick, Font = new Font("Segoe UI", 14, FontStyle.Bold), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
            Controls.Add(lblTongTien);
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            try
            {
                cboHangHoa.DataSource = db.HangHoas.OrderBy(h => h.TenHH).ToList();
                cboHangHoa.DisplayMember = "TenHH";
                cboHangHoa.ValueMember = "MaHH";
                HienThiDanhSachMua();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được cơ sở dữ liệu. Hãy chạy database/QLBH.sql và kiểm tra App.config.\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboHangHoa.SelectedValue == null) return;
            int ma = Convert.ToInt32(cboHangHoa.SelectedValue);
            int soLuong = Decimal.ToInt32(nudSoLuong.Value);
            var daChon = dsmua.SingleOrDefault(x => x.MaHH == ma);
            if (daChon != null)
            {
                daChon.SoLuong = soLuong;
                MessageBox.Show("Đã cập nhật số lượng mặt hàng.");
            }
            else
            {
                var hh = db.HangHoas.SingleOrDefault(x => x.MaHH == ma);
                if (hh == null) { MessageBox.Show("Không tìm thấy hàng hóa."); return; }
                dsmua.Add(new HangChonMua { MaHH = hh.MaHH, TenHH = hh.TenHH, DonGia = hh.DonGia, SoLuong = soLuong });
            }
            HienThiDanhSachMua();
        }

        private void HienThiDanhSachMua()
        {
            dgvHangHoa.DataSource = null;
            dgvHangHoa.DataSource = dsmua.Select(x => new {
                MaHH = x.MaHH, TenHH = x.TenHH, SoLuong = x.SoLuong,
                DonGia = x.DonGia, ThanhTien = x.ThanhTien
            }).ToList();
            lblTongTien.Text = dsmua.Sum(x => x.ThanhTien).ToString("#,##0.##") + " đ";
        }

        private void btnLapHoaDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKhachHang.Text)) { MessageBox.Show("Vui lòng nhập tên khách hàng."); return; }
            if (dsmua.Count == 0) { MessageBox.Show("Vui lòng thêm ít nhất một mặt hàng."); return; }

            try
            {
                var hd = new HoaDon { NgayLapHD = DateTime.Now, KhachHang = txtKhachHang.Text.Trim(), TongTien = dsmua.Sum(x => x.ThanhTien) };
                db.HoaDons.InsertOnSubmit(hd);
                db.SubmitChanges();

                foreach (var item in dsmua)
                    db.CTHoaDons.InsertOnSubmit(new CTHoaDon { MaHD = hd.MaHD, MaHH = item.MaHH, SoLuong = item.SoLuong });
                db.SubmitChanges();

                using (var report = new frmReport(hd.MaHD, hd.NgayLapHD, hd.KhachHang, dsmua.ToList(), hd.TongTien))
                    report.ShowDialog(this);

                MessageBox.Show("Đã lưu hóa đơn thành công.");
                txtKhachHang.Clear();
                dsmua.Clear();
                HienThiDanhSachMua();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lập hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
