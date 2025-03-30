using Microsoft.AspNetCore.Mvc;
using NhaHang.Models;
using NhaHang.Data;
using System.Linq;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Register(string Email, string password, string hoTen, string gioiTinh)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Message = "Email và mật khẩu không được để trống!";
            return View();
        }

        Email = Email.Trim().ToLower(); // Chuẩn hóa email

        if (_context.Users.Any(u => u.Email == Email))
        {
            ViewBag.Message = "Email đã tồn tại!";
            return View();
        }

        var user = new Users
        {
            Email = Email,
            MatKhau = password,
            HoTen = hoTen,
            GioiTinh = gioiTinh
            // Không cần gán VaiTro, SQL Server tự động xử lý
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        ViewBag.Message = "Đăng ký thành công!";
        return RedirectToAction("Login");
    }
}
