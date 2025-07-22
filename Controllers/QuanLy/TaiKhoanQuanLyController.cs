using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using NhaHang.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace NhaHang.Controllers.QuanLy
{
    public class TaiKhoanQuanLyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Users> _passwordHasher;
        public TaiKhoanQuanLyController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Users>();
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
        public IActionResult ThemTaiKhoan(IFormCollection form)
        {
            string emailPrefix = form["EmailPrefix"];
            string vaiTro = form["VaiTro"];
            string emailSuffix = vaiTro switch
            {
                "NhanVienPhucVu" => "@nvpv.com",
                "NhanVienBep" => "@nvb.com",
                "QuanLy" => "@admin.com",
                "KhachHang" => "@gmail.com",
                _ => "@default"
            };

            string emailFull = emailPrefix + emailSuffix;

            var user = new Users
            {
                HoTen = form["HoTen"],
                GioiTinh = form["GioiTinh"],
                SDT = form["SDT"],
                DiaChi = form["DiaChi"],
                Email = emailFull,
                VaiTro = vaiTro,
                TrangThai = "Không Hoạt Động",
                Image = null,
                MatKhau = HashPassword(form["MatKhau"])  
            };

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["ThongBao"] = "Thêm Tài Khoản Thành Công";
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Vui lòng nhập đầy đủ và đúng thông tin!";
            return View("~/Views/QuanLy/QuanLyTaiKhoan/ThemTaiKhoan.cshtml", user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        [HttpGet]
        public IActionResult ThemTaiKhoanView(string vaiTro)
        {
            ViewBag.VaiTro = vaiTro;
            return View("~/Views/QuanLy/QuanLyTaiKhoan/ThemTaiKhoan.cshtml");
        }

        [HttpPost]
        public IActionResult XoaTaiKhoan(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();

                TempData["ThongBao"] = "Xóa Tài Khoản Thành Công";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChinhSuaTaiKhoan(Users updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user == null) return NotFound();

            user.HoTen = updatedUser.HoTen;
            user.GioiTinh = updatedUser.GioiTinh;
            user.SDT = updatedUser.SDT;
            user.DiaChi = updatedUser.DiaChi;
            user.Email = updatedUser.Email;
            user.VaiTro = updatedUser.VaiTro;
            user.TrangThai = updatedUser.TrangThai;

            if (!string.IsNullOrWhiteSpace(updatedUser.MatKhau))
            {
                user.MatKhau = HashPassword(updatedUser.MatKhau);
            }

            _context.SaveChanges();
            TempData["ThongBao"] = " Chỉnh Sửa Tài Khoản Thành Công";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChinhSuaTaiKhoanView(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            ViewBag.VaiTro = user.VaiTro;
            return View("~/Views/QuanLy/QuanLyTaiKhoan/ChinhSuaTaiKhoan.cshtml", user);
        }
    }
}
