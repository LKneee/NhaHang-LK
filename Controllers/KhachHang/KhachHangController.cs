using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using NhaHang.Models;
using System.Linq;

namespace NhaHang.Controllers.KhachHang
{
    public class KhachHangController : Controller
    {
        private readonly AppDbContext _context;

        public KhachHangController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            var list = _context.Menu.ToList();
            return View("Menu/Index", list);
        }

        public IActionResult Combo()
        {
            return View("Menu/Combo");
        }

        public IActionResult GiamGia()
        {
            return View("Menu/GiamGia");
        }

        public IActionResult SpecialDay()
        {
            return View("Menu/SpecialDay");
        }
    }
}

