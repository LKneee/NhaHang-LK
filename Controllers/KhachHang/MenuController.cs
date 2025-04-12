using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaHang.Data;
using System.Threading.Tasks;
using System.Linq;

namespace NhaHang.Controllers.KhachHang
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
        {
            _context = context;
        }


        // GET: KhachHang/Menu
        public async Task<IActionResult> Index(int? categoryId)
        {
            var menu = _context.Menu.AsQueryable();

            if (categoryId.HasValue)
            {
                menu = menu.Where(m => m.CategoryId == categoryId);
            }

            return View("Views/Menu/Index.cshtml", await menu.ToListAsync());
        }

        // GET: KhachHang/Menu/GiamGia
        public async Task<IActionResult> GiamGia()
        {
            var items = await _context.Menu
                .Where(m => m.GiamGia.HasValue && m.GiamGia > 0)
                .ToListAsync();

            return View("Views/KhachHang/Menu/GiamGia.cshtml", items);
        }

        // GET: KhachHang/Menu/SpecialDay
        public async Task<IActionResult> SpecialDay()
        {
            var today = DateTime.Today;
            var items = await _context.Menu
                .Where(m => m.SpecialDay.HasValue && m.SpecialDay.Value.Date == today)
                .ToListAsync();

            return View("Views/KhachHang/Menu/SpecialDay.cshtml", items);
        }

        // GET: KhachHang/Menu/Combo?size=2
        public async Task<IActionResult> Combo(int size = 2)
        {
            var items = await _context.Menu
                .Where(m => m.Combo.HasValue && m.Combo == size)
                .ToListAsync();

            return View("Views/KhachHang/Menu/Combo.cshtml", items);
        }
    }
}
