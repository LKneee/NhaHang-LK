using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaHang.Data;

namespace NhaHang.Controllers.NhanVienBep
{
    public class NhanVienBepController : Controller
    {
        private readonly AppDbContext _context;

        public NhanVienBepController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy tất cả đơn hàng có trạng thái "Chờ bếp" và include OrderItems
            var orders = _context.Orders
                .Include(o => o.OrderItem)
                .Where(o => o.TrangThai == "Chờ bếp" || o.TrangThai == "Đã hoàn tất")
                .OrderByDescending(o => o.NgayDat)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult HoanTat(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItem)
                .FirstOrDefault(o => o.OrderId == id);

            if (order != null)
            {
                // Cập nhật tất cả các món trong đơn hàng thành "Hoàn tất"
                foreach (var item in order.OrderItem)
                {
                    item.TrangThai = "Hoàn tất";
                }

                // Sau đó cập nhật trạng thái đơn hàng
                order.TrangThai = "Đã hoàn tất";
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult HoanTatMon(int itemId)
        {
            var item = _context.OrderItem.FirstOrDefault(i => i.OrderItemId == itemId);
            if (item != null)
            {
                item.TrangThai = "Hoàn tất";
                _context.SaveChanges();

                // Nếu tất cả món của đơn hàng đã hoàn tất thì cập nhật luôn trạng thái đơn
                var order = _context.Orders
                    .Include(o => o.OrderItem)
                    .FirstOrDefault(o => o.OrderId == item.OrderId);

                if (order != null && order.OrderItem.All(i => i.TrangThai == "Hoàn tất"))
                {
                    order.TrangThai = "Đã hoàn tất";
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult GetDonHangMoi()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItem)
                .Where(o => o.TrangThai == "Chờ bếp" || o.TrangThai == "Đã hoàn tất")
                .OrderByDescending(o => o.NgayDat)
                .Select(o => new {
                    o.OrderId,
                    o.Ban,
                    NhanVienHoTen = o.NhanVienHoTen ?? "", // lấy trực tiếp từ cột đã lưu
                    NgayDat = o.NgayDat.ToString("dd/MM/yyyy HH:mm"),
                    orderType = o.OrderType,
                    o.GhiChu,
                    o.TrangThai,
                    OrderItems = o.OrderItem.Select(i => new {
                        i.OrderItemId,
                        i.TenMon,
                        i.SoLuong,
                        i.TrangThai
                    }).ToList()
                })
                .ToList();

            return Json(orders);
        }



    }
}
