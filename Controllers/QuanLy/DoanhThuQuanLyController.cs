using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaHang.Data;
using System.Globalization;

namespace NhaHang.Controllers.QuanLy
{
    public class DoanhThuQuanLyController : Controller
    {
        private readonly AppDbContext _context;

        public DoanhThuQuanLyController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("~/Views/QuanLy/QuanLyDoanhThu/Index.cshtml");
        }

        [HttpGet]
        public IActionResult GetDoanhThuTheoNgay()
        {
            var doanhThuTheoNgay = _context.Orders
                .Where(o => o.ThanhToan == "Đã thanh toán")
                .GroupBy(o => o.NgayDat.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    TongTien = g.Sum(o => o.OrderItem.Sum(i => i.DonGia * i.SoLuong))
                })
                .OrderBy(g => g.Ngay)
                .Select(g => new
                {
                    Ngay = g.Ngay.ToString("dd/MM/yyyy"),
                    TongTien = g.TongTien
                })
                .ToList();

            return Json(doanhThuTheoNgay);
        }

        [HttpGet]
        public IActionResult GetDoanhThuTheoTuan()
        {
            var today = _context.Orders
                .Where(o => o.ThanhToan == "Đã thanh toán")
                .Select(o => o.NgayDat.Date)
                .OrderByDescending(d => d)
                .FirstOrDefault();

            var startDate = today.AddDays(-6);

            var orders = _context.Orders
                .Where(o => o.ThanhToan == "Đã thanh toán" && o.NgayDat.Date >= startDate && o.NgayDat.Date <= today)
                .Include(o => o.OrderItem)
                .AsEnumerable();

            var doanhThuTheoTuan = orders
                .GroupBy(o => o.NgayDat.Date)
                .Select(g => new
                {
                    Ngay = g.Key.ToString("dd/MM/yyyy"),
                    TongTien = g.Sum(o => o.OrderItem.Sum(i => i.DonGia * i.SoLuong))
                })
                .OrderBy(g => DateTime.ParseExact(g.Ngay, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .ToList();

            return Json(doanhThuTheoTuan);
        }



        [HttpGet]
        public IActionResult GetDoanhThuTheoThang()
        {
            var ordersDaThanhToan = _context.Orders
                .Where(o => o.ThanhToan == "Đã thanh toán")
                .Include(o => o.OrderItem)
                .AsEnumerable();  

            var doanhThuTheoThang = ordersDaThanhToan
                .GroupBy(o => new DateTime(o.NgayDat.Year, o.NgayDat.Month, 1))
                .Select(g => new
                {
                    thang = g.Key.ToString("MM/yyyy"),
                    tongTien = g.Sum(o => o.OrderItem.Sum(i => i.DonGia * i.SoLuong))
                })
                .OrderBy(g => g.thang)
                .ToList();

            return Json(doanhThuTheoThang);
        }


        [HttpGet]
        public IActionResult GetDoanhThuTheoNam()
        {
            var doanhThuTheoNam = _context.Orders
                .Where(o => o.ThanhToan == "Đã thanh toán")
                .GroupBy(o => o.NgayDat.Year)
                .Select(g => new
                {
                    Nam = g.Key,
                    TongTien = g.Sum(o => o.OrderItem.Sum(i => i.DonGia * i.SoLuong))
                })
                .OrderBy(g => g.Nam)
                .ToList();

            return Json(doanhThuTheoNam);
        }
    }
}
