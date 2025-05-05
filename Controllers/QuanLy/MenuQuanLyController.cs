using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using NhaHang.Models;
using System.Linq;

namespace NhaHang.Controllers.QuanLy
{
    public class MenuQuanLyController : Controller
    {
        private readonly AppDbContext _context;

        public MenuQuanLyController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách món ăn
        public IActionResult Index()
        {
            var menuList = _context.Menu.ToList();
            return View("~/Views/QuanLy/QuanLyMenu/Index.cshtml", menuList);
        }

        // Hiển thị form thêm
        public IActionResult Create()
        {
            return View("~/Views/QuanLy/QuanLyMenu/MenuForm.cshtml", new Menu());
        }

        // Xử lý thêm món
        [HttpPost]
        public IActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Menu.Add(menu);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Views/QuanLy/QuanLyMenu/MenuForm.cshtml", menu);
        }

        // Hiển thị form sửa
        public IActionResult Edit(int id)
        {
            var menu = _context.Menu.FirstOrDefault(m => m.Id == id);
            if (menu == null) return NotFound();
            return View("~/Views/QuanLy/QuanLyMenu/MenuForm.cshtml", menu);
        }

        // Xử lý sửa
        [HttpPost]
        public IActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Menu.Update(menu);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Views/QuanLy/QuanLyMenu/MenuForm.cshtml", menu);
        }

        // Xóa món
        public IActionResult Delete(int id)
        {
            var menu = _context.Menu.FirstOrDefault(m => m.Id == id);
            if (menu != null)
            {
                _context.Menu.Remove(menu);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
