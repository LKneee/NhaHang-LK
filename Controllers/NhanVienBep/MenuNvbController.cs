using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaHang.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NhaHang.Controllers.NhanVienBep
{
    public class MenuNvbController : Controller
    {
        private readonly AppDbContext _context;

        public MenuNvbController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var menu = _context.Menu.AsQueryable();

            if (categoryId.HasValue)
            {
                menu = menu.Where(m => m.CategoryId == categoryId);
            }

            return View("Views/NhanVienBep/TrangThaiMonAn/Index.cshtml", await menu.ToListAsync());
        }

        [Route("MenuNvb/HaiSan")]
        public async Task<IActionResult> HaiSan()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 1)
                .ToListAsync();
            return View("Views/NhanVienBep/TrangThaiMonAn/Index.cshtml", items);
        }

        [Route("MenuNvb/Ga")]
        public async Task<IActionResult> Ga()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 2)
                .ToListAsync();
            return View("Views/NhanVienBep/TrangThaiMonAn/Index.cshtml", items);
        }

        [Route("MenuNvb/Bo")]
        public async Task<IActionResult> Bo()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 3)
                .ToListAsync();
            return View("Views/NhanVienBep/TrangThaiMonAn/Index.cshtml", items);
        }

        [Route("MenuNvb/Salad")]
        public async Task<IActionResult> Salad()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 4)
                .ToListAsync();
            return View("Views/NhanVienBep/TrangThaiMonAn/Index.cshtml", items);
        }

        [Route("MenuNvb/TrangMieng")]
        public async Task<IActionResult> TrangMieng()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 5)
                .ToListAsync();
            return View("Views/NhanVienBep/TrangThaiMonAn/Index.cshtml", items);
        }
    }
}
