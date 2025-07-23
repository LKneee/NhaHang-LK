using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaHang.Data;
using NhaHang.Models;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace NhaHang.Controllers.QuanLy
{
    public class DonHangQuanLyController : Controller
    {
        private readonly AppDbContext _context;

        public DonHangQuanLyController(AppDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItem)
                .OrderByDescending(o => o.NgayDat)
                .ToList();

            return View("~/Views/QuanLy/QuanLyDonHang/Index.cshtml", orders);
        }

        
        [HttpPost]
        public IActionResult ThanhToan(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItem)
                .FirstOrDefault(o => o.OrderId == id);

            if (order != null)
            {
                order.ThanhToan = "Đã thanh toán";
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult DonHangMoi(string ngay)
        {
            DateTime parsedNgay;

            if (!DateTime.TryParse(ngay, out parsedNgay))
            {
                parsedNgay = DateTime.Today; 
            }

            var orders = _context.Orders
                .Include(o => o.OrderItem)
                .Where(o => o.NgayDat.Date == parsedNgay.Date) 
                .OrderByDescending(o => o.NgayDat)
                .Select(o => new
                {
                    orderId = o.OrderId,
                    ban = o.Ban,
                    nhanVienHoTen = o.NhanVienHoTen,
                    ngayDat = o.NgayDat.ToString("dd/MM/yyyy HH:mm"),
                    orderType = o.OrderType,
                    ghiChu = o.GhiChu,
                    thanhToan = o.ThanhToan,
                    tongTien = o.OrderItem.Sum(i => i.DonGia * i.SoLuong),
                    orderItems = o.OrderItem.Select(i => new
                    {
                        tenMon = i.TenMon,
                        soLuong = i.SoLuong
                    })
                })
                .ToList();

            return Json(orders);
        }

        [HttpPost]
        public IActionResult InHoaDon(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItem)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 36, 36, 36, 36);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font titleFont = new Font(baseFont, 20, Font.BOLD);
                Font subTitleFont = new Font(baseFont, 14, Font.BOLD);
                Font normalFont = new Font(baseFont, 12, Font.NORMAL);
                Font boldFont = new Font(baseFont, 12, Font.BOLD);
                Font footerFont = new Font(baseFont, 12, Font.ITALIC);

                
                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/logo.jpg");
                if (System.IO.File.Exists(logoPath))
                {
                    var logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(100, 100);
                    logo.Alignment = Element.ALIGN_CENTER;
                    logo.SpacingAfter = 10;
                    doc.Add(logo);
                }

                
                Paragraph title = new Paragraph("NHÀ HÀNG LK", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                Paragraph subTitle = new Paragraph("HÓA ĐƠN THANH TOÁN", subTitleFont);
                subTitle.Alignment = Element.ALIGN_CENTER;
                subTitle.SpacingAfter = 20;
                doc.Add(subTitle);

                
                // THÔNG TIN HÓA ĐƠN - Chia trái phải
                PdfPTable infoContainer = new PdfPTable(2);
                infoContainer.WidthPercentage = 100;
                infoContainer.SetWidths(new float[] { 1f, 1f });
                infoContainer.DefaultCell.Border = Rectangle.NO_BORDER;

                
                PdfPTable leftInfo = new PdfPTable(1);
                leftInfo.DefaultCell.Border = Rectangle.NO_BORDER;
                leftInfo.AddCell(new PdfPCell(new Phrase($"Mã hóa đơn: {order.OrderId}", normalFont)) { Border = Rectangle.NO_BORDER, PaddingBottom = 4 });
                leftInfo.AddCell(new PdfPCell(new Phrase($"Nhân viên: {order.NhanVienHoTen}", normalFont)) { Border = Rectangle.NO_BORDER });

                
                PdfPTable rightInfo = new PdfPTable(1);
                rightInfo.DefaultCell.Border = Rectangle.NO_BORDER;
                rightInfo.AddCell(new PdfPCell(new Phrase($"Ngày: {order.NgayDat:dd/MM/yyyy HH:mm}", normalFont)) { Border = Rectangle.NO_BORDER, PaddingBottom = 4 });
                rightInfo.AddCell(new PdfPCell(new Phrase($"Bàn: {order.Ban}", normalFont)) { Border = Rectangle.NO_BORDER });
                
                PdfPCell leftCell = new PdfPCell(leftInfo);
                leftCell.Border = Rectangle.NO_BORDER;

                PdfPCell rightCell = new PdfPCell(rightInfo);
                rightCell.Border = Rectangle.NO_BORDER;

                infoContainer.AddCell(leftCell);
                infoContainer.AddCell(rightCell);
                infoContainer.SpacingAfter = 20;

                doc.Add(infoContainer);

                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 4f, 1f, 2f, 2f });

                string[] headers = { "Tên món", "Số lượng", "Đơn giá", "Thành tiền" };
                foreach (var header in headers)
                {
                    var headerCell = new PdfPCell(new Phrase(header, boldFont))
                    {
                        BackgroundColor = new BaseColor(230, 230, 230),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 6
                    };
                    table.AddCell(headerCell);
                }

                foreach (var item in order.OrderItem)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.TenMon, normalFont)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(item.SoLuong.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase($"{item.DonGia:N0} VND", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase($"{(item.DonGia * item.SoLuong):N0} VND", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
                }

                doc.Add(table);

                
                Paragraph tongTien = new Paragraph($"Tổng tiền: {order.TongTien:N0} VND", boldFont);
                tongTien.Alignment = Element.ALIGN_RIGHT;
                tongTien.SpacingBefore = 15;
                tongTien.SpacingAfter = 10;
                doc.Add(tongTien);

                
                if (!string.IsNullOrWhiteSpace(order.GhiChu))
                {
                    Paragraph note = new Paragraph($"Ghi chú: {order.GhiChu}", normalFont);
                    note.SpacingBefore = 10;
                    doc.Add(note);
                }

                
                Paragraph thank = new Paragraph("★ Cảm ơn quý khách và hẹn gặp lại! ★", footerFont);
                thank.Alignment = Element.ALIGN_CENTER;
                thank.SpacingBefore = 30;
                doc.Add(thank);

                doc.Close();
                return File(ms.ToArray(), "application/pdf", $"HoaDon_{order.OrderId}.pdf");
            }
        }

    }
}
