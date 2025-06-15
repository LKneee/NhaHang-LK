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
                parsedNgay = DateTime.Today; // Nếu không truyền hoặc lỗi format thì lấy hôm nay
            }

            var orders = _context.Orders
                .Include(o => o.OrderItem)
                .Where(o => o.NgayDat.Date == parsedNgay.Date) // lọc theo ngày
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
                Document doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

              
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12, Font.NORMAL);

              
                doc.Add(new Paragraph("--- Nhà Hàng LK ---", font));
                doc.Add(new Paragraph("HÓA ĐƠN TÍNH TIỀN", font));
                doc.Add(new Paragraph($"Thời Gian Vào: {order.NgayDat:dd/MM/yyyy HH:mm}", font));
                doc.Add(new Paragraph($"Nhân Viên: {order.NhanVienHoTen}", font));
                doc.Add(new Paragraph(" ", font));

               
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;

                
                table.AddCell(new PdfPCell(new Phrase("Tên Món", font)));
                table.AddCell(new PdfPCell(new Phrase("Số Lượng", font)));
                table.AddCell(new PdfPCell(new Phrase("Đơn Giá", font)));
                table.AddCell(new PdfPCell(new Phrase("Thành Tiền", font)));

                foreach (var item in order.OrderItem)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.TenMon, font)));
                    table.AddCell(new PdfPCell(new Phrase(item.SoLuong.ToString(), font)));
                    table.AddCell(new PdfPCell(new Phrase($"{item.DonGia:N0} VND", font)));
                    table.AddCell(new PdfPCell(new Phrase($"{(item.DonGia * item.SoLuong):N0} VND", font)));
                }

                doc.Add(table);
                doc.Add(new Paragraph(" ", font));
                doc.Add(new Paragraph($"Tổng tiền: {order.TongTien:N0} VND", font));
                doc.Add(new Paragraph(" ", font));
                doc.Add(new Paragraph("--- Cảm ơn và hẹn gặp lại bạn ---", font));

                doc.Close();

                return File(ms.ToArray(), "application/pdf", $"HoaDon_{order.OrderId}.pdf");
            }
        }


    }
}
