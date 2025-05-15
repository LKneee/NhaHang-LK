using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NhaHang.Data; 
using NhaHang.Models;
using System;
using System.Linq;

namespace NhaHang.Controllers.NhanVienPhucVu
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromBody] DonHang model)
        {
            if (model == null || model.phieu == null || !model.phieu.Any())
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var order = new Order
            {
                Ban = model.banSo,
                NgayDat = DateTime.Now,
                NhanVienHoTen = HttpContext.Session.GetString("UserName"),
                NhanVienEmail = HttpContext.Session.GetString("UserEmail"),
                GhiChu = model.ghiChu,
                OrderType = model.orderType,
                TrangThai = "Chờ bếp",
                TongTien = model.phieu.Sum(x => x.gia * x.soluong)
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // để lấy được OrderId

            foreach (var item in model.phieu)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    TenMon = item.ten,
                    SoLuong = item.soluong,
                    DonGia = item.gia
                };
                _context.OrderItem.Add(orderItem);
                Console.WriteLine($"Đã thêm {orderItem.TenMon}");
            }

            _context.SaveChanges();

            return Ok(new { message = "Đặt món thành công!" });
        }
    }
}
