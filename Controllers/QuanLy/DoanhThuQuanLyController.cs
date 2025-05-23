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
        public IActionResult GetDoanhThuTheoNgayChon(string date)
        {
            if (!DateTime.TryParse(date, out DateTime ngayChon))
                return BadRequest("Ngày không hợp lệ");

            var startDate = ngayChon.AddDays(-6);
            var endDate = ngayChon;

            var orders = _context.Orders
                .Where(o => o.ThanhToan == "Đã thanh toán" && o.NgayDat.Date >= startDate && o.NgayDat.Date <= endDate)
                .Include(o => o.OrderItem)
                .AsEnumerable();

            var doanhThuTheoNgay = orders
                .GroupBy(o => o.NgayDat.Date)
                .Select(g => new
                {
                    Ngay = g.Key.ToString("dd/MM/yyyy"),
                    TongTien = g.Sum(o => o.OrderItem.Sum(i => i.DonGia * i.SoLuong))
                })
                .ToList();

            var ketQuaDayDu = new List<object>();
            for (int i = 6; i >= 0; i--)
            {
                var ngay = endDate.AddDays(-i).Date;
                var item = doanhThuTheoNgay.FirstOrDefault(d => d.Ngay == ngay.ToString("dd/MM/yyyy"));
                ketQuaDayDu.Add(new
                {
                    Ngay = ngay.ToString("dd/MM/yyyy"),
                    TongTien = item?.TongTien ?? 0
                });
            }

            return Json(ketQuaDayDu);
        }

        [HttpGet]
        public IActionResult GetDoanhThuTheoThang(string date)
        {
            if (!DateTime.TryParse(date, out DateTime ngayChon))
                return BadRequest("Ngày không hợp lệ");

            int nam = ngayChon.Year;
            int thangHienTai = ngayChon.Month;

            var orders = _context.Orders
                .Where(o => o.ThanhToan == "Đã thanh toán" && o.NgayDat.Year == nam && o.NgayDat.Month <= thangHienTai)
                .Include(o => o.OrderItem)
                .AsEnumerable();

            var doanhThuTheoThang = orders
                .GroupBy(o => o.NgayDat.Month)
                .Select(g => new
                {
                    Thang = $"Tháng {g.Key}",
                    TongTien = g.Sum(o => o.OrderItem.Sum(i => i.DonGia * i.SoLuong))
                })
                .OrderBy(g => g.Thang)
                .ToList();

            var ketQuaDayDu = new List<object>();
            for (int thang = 1; thang <= thangHienTai; thang++)
            {
                var item = doanhThuTheoThang.FirstOrDefault(d => d.Thang == $"Tháng {thang}");
                ketQuaDayDu.Add(new
                {
                    Thang = $"Tháng {thang}",
                    TongTien = item?.TongTien ?? 0
                });
            }

            return Json(ketQuaDayDu);
        }


        [HttpGet]
        public IActionResult GetDoanhThuTheoNam(string date)
        {
            if (!DateTime.TryParse(date, out DateTime ngayChon))
                return BadRequest("Ngày không hợp lệ");

            int nam = ngayChon.Year;
            int namBatDau = nam - 10;

            var orders = _context.Orders
                .Where(o => o.ThanhToan == "Đã thanh toán" && o.NgayDat.Year >= namBatDau && o.NgayDat.Year <= nam)
                .Include(o => o.OrderItem)
                .AsEnumerable();

            var doanhThuTheoNam = orders
                .GroupBy(o => o.NgayDat.Year)
                .Select(g => new
                {
                    Nam = g.Key,
                    TongTien = g.Sum(o => o.OrderItem.Sum(i => i.DonGia * i.SoLuong))
                })
                .ToList();

            var ketQua = new List<object>();
            for (int i = namBatDau; i <= nam; i++)
            {
                var item = doanhThuTheoNam.FirstOrDefault(d => d.Nam == i);
                ketQua.Add(new
                {
                    Nam = i.ToString(),
                    TongTien = item?.TongTien ?? 0
                });
            }

            return Json(ketQua);
        }


    }
}
