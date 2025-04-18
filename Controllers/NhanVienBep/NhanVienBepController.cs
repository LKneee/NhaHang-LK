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
                .Where(o => o.TrangThai == "Chờ bếp")
                .OrderByDescending(o => o.NgayDat)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult HoanTat(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order != null)
            {
                order.TrangThai = "Đã hoàn tất";
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
