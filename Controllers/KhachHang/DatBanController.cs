using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using NhaHang.Models;
using System;

namespace NhaHang.Controllers.KhachHang
{
    public class DatBanController : Controller
    {
        private readonly AppDbContext _context;

        public DatBanController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (TempData["ThongBao"] != null)
            {
                ViewBag.ThongBao = TempData["ThongBao"];
            }

            return View("~/Views/KhachHang/DatBan/Index.cshtml");
        }

        [HttpPost]
        public IActionResult Index(DatBan model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/KhachHang/DatBan/Index.cshtml", model);
            }

            if (model.NgayDat.Date == DateTime.Now.Date && DateTime.Now.Hour >= 21)
            {
                ModelState.AddModelError("NgayChon", "Không còn chỗ cho ngày bạn đặt.");
                return View("~/Views/KhachHang/DatBan/Index.cshtml", model);
            }

            model.NgayTao = DateTime.Now;

            _context.DatBan.Add(model);
            _context.SaveChanges();


            TempData["ThongBao"] = "Đặt bàn thành công!";


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetDanhSachDatBan(string ngayLoc)
        {
            DateTime? ngayFilter = null;
            if (!string.IsNullOrEmpty(ngayLoc))
            {
                if (DateTime.TryParse(ngayLoc, out var dt))
                {
                    ngayFilter = dt.Date;
                }
            }

            var danhSach = _context.DatBan.AsQueryable();

            if (ngayFilter.HasValue)
            {
                danhSach = danhSach.Where(d => d.NgayDat.Date == ngayFilter.Value);
            }

            var result = danhSach.Select(d => new {
                id = d.Id,
                hoTen = d.HoTen,
                soDienThoai = d.SoDienThoai,
                ngayDat = d.NgayDat,
                gioNhanBan = d.GioNhanBan,
                loaiTiec = d.LoaiTiec,
                soNguoi = d.SoNguoi,
                ngayTao = d.NgayTao,
                trangThai = d.TrangThai ?? "Chưa Đến"
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public IActionResult CapNhatTrangThai(int id, string trangThai)
        {
            var datBan = _context.DatBan.Find(id);
            if (datBan == null)
            {
                return NotFound();
            }

            datBan.TrangThai = trangThai;
            _context.SaveChanges();

            return Ok();
        }


    }
}
