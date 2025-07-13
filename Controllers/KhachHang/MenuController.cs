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
        public async Task<IActionResult> Index(int? categoryId)
        {
            var menu = _context.Menu.AsQueryable();

            if (categoryId.HasValue)
            {
                menu = menu.Where(m => m.CategoryId == categoryId);
            }

            return View("Views/KhachHang/Menu/Index.cshtml", await menu.ToListAsync());
        }

        [HttpGet]
        [Route("Menu/GetDanhSachMonAn")]
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
        [Route("Menu/GetDanhSachMonAn/{category}")]
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
    }
}
