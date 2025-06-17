using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaHang.Data;

namespace NhaHang.Controllers.NhanVienPhucVu
{
    public class NhanVienPhucVuController : Controller
    {
        private readonly AppDbContext _context;

        public NhanVienPhucVuController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Where(o => o.ThanhToan == "Chưa thanh toán" || o.ThanhToan == "Đã thanh toán")
                .GroupBy(o => o.Ban)
                .Select(g => g.OrderByDescending(o => o.NgayDat).FirstOrDefault())
                .ToList();

            var tableStatus = orders.ToDictionary(o => o.Ban, o => o.ThanhToan);

            ViewBag.TableStatus = tableStatus;
            return View();
        }

        [HttpGet]
        public IActionResult GetTableStatuses()
        {
            var orders = _context.Orders
                .Where(o => o.ThanhToan == "Chưa thanh toán" || o.ThanhToan == "Đã thanh toán")
                .GroupBy(o => o.Ban)
                .Select(g => g.OrderByDescending(o => o.NgayDat).FirstOrDefault())
                .ToList();

            var tableStatus = orders.ToDictionary(o => o.Ban, o => o.ThanhToan);

            return Json(tableStatus);
        }


        public IActionResult Menu()
        {
            return RedirectToAction("Index", "MenuNvpv");
        }
        public IActionResult DanhSachMonAn()
        {
            var hoTen = HttpContext.Session.GetString("UserName");
            var email = HttpContext.Session.GetString("UserEmail");

            var order = _context.Orders
              .Include(o => o.OrderItem)
              .OrderByDescending(o => o.NgayDat)
              .ToList();

            return View("DanhSachMonAn/Index", order);
        }

        public IActionResult GetThongBaoCount()
        {
            var hoTen = HttpContext.Session.GetString("UserName");
            var email = HttpContext.Session.GetString("UserEmail");

            var count = _context.Orders
                .Count(o => o.TrangThai == "Đã hoàn tất" && o.NhanVienHoTen == hoTen && o.NhanVienEmail == email);

            return Json(new { count });
        }

        [HttpGet]
        public IActionResult GetThongBaoMoi(string ngay)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(ngay, out parsedDate))
            {
                parsedDate = DateTime.Today;
            }

            var orders = _context.Orders
                .Include(o => o.OrderItem)
                .Where(o => o.NgayDat.Date == parsedDate.Date)
                .OrderByDescending(o => o.NgayDat)
                .Select(o => new
                {
                    ban = o.Ban,
                    ngayDat = o.NgayDat.ToString("dd/MM/yyyy HH:mm"),
                    orderType = o.OrderType,
                    ghiChu = o.GhiChu,
                    trangThai = o.TrangThai,
                    orderItems = o.OrderItem.Select(i => new
                    {
                        tenMon = i.TenMon,
                        soLuong = i.SoLuong,
                        trangThai = i.TrangThai
                    })
                })
                .ToList();

            return Json(orders);
        }

    }
}
