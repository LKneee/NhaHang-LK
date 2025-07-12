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

        [HttpGet]
        [Route("MenuNvb/GetDanhSachMonAn")]
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
        [Route("MenuNvb/GetDanhSachMonAn/{category}")]
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
    }
}
