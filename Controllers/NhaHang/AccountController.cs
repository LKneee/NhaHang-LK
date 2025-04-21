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

        Email = Email.Trim().ToLower(); 

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
            
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        ViewBag.Message = "Đăng ký thành công!";
        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult Login(string Email, string password)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Message = "Email và mật khẩu không được để trống!";
            return View();
        }

        var user = _context.Users.FirstOrDefault(u => u.Email == Email && u.MatKhau == password);

        if (user == null)
        {
            ViewBag.Message = "Email hoặc mật khẩu không đúng!";
            return View();
        }

        if (string.IsNullOrEmpty(user.VaiTro))
        {
            ViewBag.Message = "Tài khoản chưa có vai trò!";
            return View();
        }


        HttpContext.Session.SetString("UserEmail", user.Email);
        HttpContext.Session.SetString("UserRole", user.VaiTro);


        return user.VaiTro switch
        {
            "KhachHang" => RedirectToAction("Index", "KhachHang"),
            "QuanLy" => RedirectToAction("Index", "QuanLy"),
            "NhanVienPhucVu" => RedirectToAction("Index", "NhanVienPhucVu"),
            "NhanVienBep" => RedirectToAction("Index", "NhanVienBep"),
            _ => RedirectToAction("Index", "Home") // Nếu VaiTro không hợp lệ, về Home
        };
    }

}
