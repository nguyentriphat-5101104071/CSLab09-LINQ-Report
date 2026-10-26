using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Configuration;

namespace CSLab09_LINQ_Report
{
    [Database(Name = "QLBH")]
    public class QLBHDataContext : DataContext
    {
        public QLBHDataContext() : base(ConfigurationManager.ConnectionStrings["QLBH"].ConnectionString) { }
        public Table<HangHoa> HangHoas { get { return GetTable<HangHoa>(); } }
        public Table<HoaDon> HoaDons { get { return GetTable<HoaDon>(); } }
        public Table<CTHoaDon> CTHoaDons { get { return GetTable<CTHoaDon>(); } }
    }

    [Table(Name = "dbo.HangHoa")]
    public class HangHoa
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int MaHH { get; set; }
        [Column] public string TenHH { get; set; }
        [Column] public string DonViTinh { get; set; }
        [Column] public double DonGia { get; set; }
    }

    [Table(Name = "dbo.HoaDon")]
    public class HoaDon
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int MaHD { get; set; }
        [Column] public DateTime NgayLapHD { get; set; }
        [Column] public string KhachHang { get; set; }
        [Column] public double TongTien { get; set; }
    }

    [Table(Name = "dbo.CTHoaDon", IsPrimaryKey = false)]
    public class CTHoaDon
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = false)] public int MaHD { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = false)] public int MaHH { get; set; }
        [Column] public int SoLuong { get; set; }
    }

    public class HangChonMua
    {
        public int MaHH { get; set; }
        public string TenHH { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien { get { return SoLuong * DonGia; } }
    }
}
