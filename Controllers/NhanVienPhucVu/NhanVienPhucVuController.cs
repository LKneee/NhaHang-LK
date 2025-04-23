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
            return View();
        }

        public IActionResult Menu()
        {
            return RedirectToAction("Index", "MenuNvpv");
        }

        // Gọi từ layout để đếm số thông báo
        public IActionResult ThongBao()
        {
            var hoTen = HttpContext.Session.GetString("UserName");
            var email = HttpContext.Session.GetString("UserEmail");

            var order = _context.Orders
              .Include(o => o.OrderItem)
              .OrderByDescending(o => o.NgayDat)
              .ToList();

            return View("ThongBao/Index", order);
        }

        // Đếm số đơn "Đã xong" để hiển thị badge
        public IActionResult GetThongBaoCount()
        {
            var hoTen = HttpContext.Session.GetString("UserName");
            var email = HttpContext.Session.GetString("UserEmail");

            var count = _context.Orders
                .Count(o => o.TrangThai == "Đã hoàn tất" && o.NhanVienHoTen == hoTen && o.NhanVienEmail == email);

            return Json(new { count });
        }

        [HttpGet]
        public IActionResult GetThongBaoMoi()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItem)
                .OrderByDescending(o => o.NgayDat)
                .Select(o => new
                {
                    ban = o.Ban,
                    ngayDat = o.NgayDat.ToString("dd/MM/yyyy HH:mm"),
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
