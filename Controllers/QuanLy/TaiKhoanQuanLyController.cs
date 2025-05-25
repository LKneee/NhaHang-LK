using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using NhaHang.Models;

namespace NhaHang.Controllers.QuanLy
{
    public class TaiKhoanQuanLyController : Controller
    {
        private readonly AppDbContext _context;
        public TaiKhoanQuanLyController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("~/Views/QuanLy/QuanLyTaiKhoan/Index.cshtml");
        }

        public IActionResult VaiTroNguoiDung(string vaiTro)
        {
            var users = _context.Users
                .Where(u => u.VaiTro == vaiTro)
                .ToList();

            return PartialView("~/Views/QuanLy/QuanLyTaiKhoan/DanhSach.cshtml", users);

        }

        [HttpPost]
        public IActionResult ThemTaiKhoan(Users user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return Content("Success");
            }
            return BadRequest("Lỗi dữ liệu");
        }

        [HttpDelete]
        public IActionResult XoaTaiKhoan(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("Đã xóa");
        }

        public IActionResult SuaTaiKhoan(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return NotFound();

            return View("SuaTaiKhoan", user);
        }

        [HttpPost]
        public IActionResult CapNhatTaiKhoan(Users user)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existing == null) return NotFound();

            existing.HoTen = user.HoTen;
            existing.GioiTinh = user.GioiTinh;
            existing.SDT = user.SDT;
            existing.DiaChi = user.DiaChi;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
