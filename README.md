# CSLab09 - LINQ + Report

Ứng dụng Windows Forms quản lý bán hàng đơn giản theo nội dung LAB 09:
- Chọn hàng hóa và số lượng.
- Nếu hàng đã có trong danh sách mua thì cập nhật số lượng.
- Tính tổng tiền.
- Lập hóa đơn và lưu hóa đơn/chi tiết vào SQL Server.
- Hiển thị bản xem trước hóa đơn để in hoặc lưu PDF.

## Mở và chạy
1. Giải nén ZIP.
2. Mở `CSLab09-LINQ-Report.csproj` bằng Visual Studio.
3. Mở SQL Server Management Studio, chạy `database/QLBH.sql`.
4. Nếu cần, sửa chuỗi kết nối trong `App.config`.
5. Build bằng `Ctrl+Shift+B`, chạy bằng `F5`.

Yêu cầu: Windows, Visual Studio với workload **.NET desktop development**, .NET Framework 4.8 Developer Pack, SQL Server LocalDB/Express.

## Cấu trúc
- `Program.cs`: điểm khởi chạy.
- `Models.cs`: các lớp LINQ to SQL.
- `frmBanHang.cs`: màn hình bán hàng, thêm hàng và lập hóa đơn.
- `frmReport.cs`: xem trước/in hóa đơn.
- `database/QLBH.sql`: tạo database và dữ liệu mẫu.

Ứng dụng dùng `PrintDocument` để xem trước/in hóa đơn, tránh phụ thuộc vào extension ReportViewer riêng. Chức năng báo cáo vẫn hiển thị tên khách hàng, danh sách hàng mua, số lượng, đơn giá, thành tiền và tổng tiền.
