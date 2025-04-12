using Microsoft.AspNetCore.Mvc;
using NhaHang.Models;
using System.Diagnostics;

namespace NhaHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (!string.IsNullOrEmpty(userRole))
            {
                return userRole switch
                {
                    "KhachHang" => RedirectToAction("Index", "KhachHang"),
                    "QuanLy" => RedirectToAction("Index", "QuanLy"),
                    "NhanVienPhucVu" => RedirectToAction("Index", "NhanVienPhucVu"),
                    "NhanVienBep" => RedirectToAction("Index", "NhanVienBep"),
                    _ => View()
                };
            }
            return View(); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
