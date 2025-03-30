using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using NhaHang.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;


namespace NhaHang.Controllers
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
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
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

            HttpContext.Session.SetString("UserRole", user.VaiTro);
            HttpContext.Session.SetString("UserName", user.HoTen);

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
