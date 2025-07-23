using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using NhaHang.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;


namespace NhaHang.Controllers.NhaHang
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    user.TrangThai = "Không Hoạt Động"; 
                    _context.SaveChanges();
                }
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(Users user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại!");
                return View();
            }

            user.MatKhau = HashPassword(user.MatKhau);
            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["ThongBao"] = "Đăng Ký Thành Công";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.MatKhau))
            {
                ModelState.AddModelError("Error", "Sai email hoặc mật khẩu!");
                return View();
            }

            user.TrangThai = "Đang Hoạt Động";
            _context.Update(user);
            _context.SaveChanges();

            string hoTen = user.HoTen ?? "Chưa cập nhật";
            string sdt = user.SDT ?? "Chưa cập nhật";
            string diaChi = user.DiaChi ?? "Chưa cập nhật";
            string gioiTinh = user.GioiTinh ?? "Chưa cập nhật";

            HttpContext.Session.SetString("UserRole", user.VaiTro);
            HttpContext.Session.SetString("UserName", hoTen);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserSDT", sdt);
            HttpContext.Session.SetString("UserDiaChi", diaChi);
            HttpContext.Session.SetString("UserGioiTinh", gioiTinh);
            HttpContext.Session.SetString("UserImage", user.Image ?? "");
            HttpContext.Session.SetString("UserVaiTro", user.VaiTro);

            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return HashPassword(inputPassword) == storedHash;
        }
    }
}
