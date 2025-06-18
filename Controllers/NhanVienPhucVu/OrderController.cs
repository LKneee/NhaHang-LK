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

            var existingOrder = _context.Orders
                .FirstOrDefault(o => o.Ban == model.banSo && o.ThanhToan == "Chưa thanh toán");

            Order order;

            if (existingOrder != null)
            {
                order = existingOrder;
                order.TrangThai = "Chờ bếp";

                if (!string.IsNullOrWhiteSpace(model.ghiChu))
                    order.GhiChu = model.ghiChu;

                if (!string.IsNullOrWhiteSpace(model.orderType))
                    order.OrderType = model.orderType;
            }
            else
            {
                order = new Order
                {
                    Ban = model.banSo,
                    NgayDat = DateTime.Now,
                    NhanVienHoTen = HttpContext.Session.GetString("UserName"),
                    NhanVienEmail = HttpContext.Session.GetString("UserEmail"),
                    GhiChu = model.ghiChu,
                    OrderType = model.orderType,
                    TrangThai = "Chờ bếp",
                    TongTien = 0
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
            }

            decimal tongTienMoi = 0;

            foreach (var item in model.phieu)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    TenMon = item.ten,
                    SoLuong = item.soluong,
                    DonGia = item.gia,
                    TrangThai = "Chờ bếp" 
                };

                tongTienMoi += item.gia * item.soluong;
                _context.OrderItem.Add(orderItem);
            }

            order.TongTien += tongTienMoi;
            _context.SaveChanges();

            return Ok(new { message = "Đặt món thành công!" });
        }

    }
}
