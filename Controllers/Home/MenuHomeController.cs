using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaHang.Data;
using System.Threading.Tasks;
using System.Linq;

namespace NhaHang.Controllers.Home
{

    public class MenuHomeController : Controller
    {

        private readonly AppDbContext _context;

        public MenuHomeController(AppDbContext context)
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

            return View("Views/Home/MenuHome/Index.cshtml", await menu.ToListAsync());
        }

        [HttpGet]
        [Route("MenuHome/GetDanhSachMonAn")]
        public IActionResult GetDanhSachMonAn()
        {
            var danhSachMon = _context.Menu
                .Select(m => new {
                    id = m.Id,
                    tenMon = m.TenMon,
                    moTa = m.MoTa,
                    gia = m.Gia,
                    image = m.Image,
                    trangThai = m.TrangThai
                })
                .ToList();

            return Json(danhSachMon);
        }

        [HttpGet]
        [Route("MenuHome/GetDanhSachMonAn/{category}")]
        public IActionResult GetDanhSachMonAnTheoLoai(string category)
        {
            int? categoryId = category.ToLower() switch
            {
                "salad" => 1,
                "haisan" => 2,
                "ga" => 3,
                "bo" => 4,
                "trangmieng" => 5,
                "nuoc" => 6,
                "nuocep" => 7,
                "sinhto" => 8,
                "ruouvangtrang" => 9,
                "ruouvangdo" => 10,
                _ => null
            };

            var query = _context.Menu.AsQueryable();
            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId);

            var danhSach = query.Select(m => new {
                id = m.Id,
                tenMon = m.TenMon,
                moTa = m.MoTa,
                gia = m.Gia,
                image = m.Image,
                trangThai = m.TrangThai
            }).ToList();

            return Json(danhSach);
        }


        //Chi Tiet
        [Route("MenuHome/{tenMonAn}/ChiTiet")]
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

            return View("Views/Home/MenuHome/ChiTiet.cshtml", monAn);
        }



        // GET: KhachHang/Menu/GiamGia
        public async Task<IActionResult> GiamGia()
        {
            var items = await _context.Menu
                .Where(m => m.GiamGia.HasValue && m.GiamGia > 0)
                .ToListAsync();

            return View("Views/Home/MenuHome/GiamGia.cshtml", items);
        }

        // GET: KhachHang/Menu/SpecialDay
        public async Task<IActionResult> SpecialDay()
        {
            var today = DateTime.Today;
            var items = await _context.Menu
                .Where(m => m.SpecialDay.HasValue && m.SpecialDay.Value.Date == today)
                .ToListAsync();

            return View("Views/Home/MenuHome/SpecialDay.cshtml", items);
        }

        // GET: KhachHang/Menu/Combo?size=2
        public async Task<IActionResult> Combo(int size = 2)
        {
            var items = await _context.Menu
                .Where(m => m.Combo.HasValue && m.Combo == size)
                .ToListAsync();

            return View("Views/Home/MenuHome/Combo.cshtml", items);
        }
    }
}
