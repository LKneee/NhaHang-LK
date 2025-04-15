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

        // Danh Muc
        public async Task<IActionResult> Index(int? categoryId)
        {
            var menu = _context.Menu.AsQueryable();

            if (categoryId.HasValue)
            {
                menu = menu.Where(m => m.CategoryId == categoryId);
            }

            return View("Views/KhachHang/Menu/Index.cshtml", await menu.ToListAsync());
        }

        [Route("Menu/HaiSan")]
        public async Task<IActionResult> HaiSan()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 1)
                .ToListAsync();
            return View("Views/KhachHang/Menu/Index.cshtml", items);
        }

        [Route("Menu/Ga")]
        public async Task<IActionResult> Ga()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 2)
                .ToListAsync();
            return View("Views/KhachHang/Menu/Index.cshtml", items);
        }

        [Route("Menu/Bo")]
        public async Task<IActionResult> Bo()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 3)
                .ToListAsync();
            return View("Views/KhachHang/Menu/Index.cshtml", items);
        }

        [Route("Menu/Salad")]
        public async Task<IActionResult> Salad()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 4)
                .ToListAsync();
            return View("Views/KhachHang/Menu/Index.cshtml", items);
        }

        [Route("Menu/TrangMieng")]
        public async Task<IActionResult> TrangMieng()
        {
            var items = await _context.Menu
                .Where(m => m.CategoryId == 5)
                .ToListAsync();
            return View("Views/KhachHang/Menu/Index.cshtml", items);
        }

        //Chi Tiet
        [Route("Menu/{tenMonAn}/ChiTiet")]
        public async Task<IActionResult> ChiTiet(string tenMonAn)
        {
            if (string.IsNullOrEmpty(tenMonAn))
                return NotFound();

            var monAn = await _context.Menu
                .FirstOrDefaultAsync(m => m.TenMon.Replace(" ", "").ToLower() == tenMonAn.ToLower());

            if (monAn == null)
            {
                return NotFound();
            }

            return View("Views/KhachHang/Menu/ChiTiet.cshtml", monAn);
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
