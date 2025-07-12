using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using System.Linq;

namespace NhaHang.Controllers.NhanVienBep
{
    public class TrangThaiMonAnController : Controller
    {
        private readonly AppDbContext _context;

        public TrangThaiMonAnController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var menu = _context.Menu.ToList();
            return View("~/Views/NhanVienBep/TrangThaiMonAn/Index.cshtml", menu);
        }

        [HttpGet]
        [Route("TrangThaiMonAn/GetDanhSachMonAn")]
        public IActionResult GetDanhSachMonAn()
        {
            var data = _context.Menu
                .Select(m => new
                {
                    id = m.Id,
                    tenMon = m.TenMon,
                    moTa = m.MoTa,
                    gia = m.Gia,
                    image = m.Image,
                    trangThai = m.TrangThai
                }).ToList();

            return Json(data);
        }

        [HttpGet]
        [Route("TrangThaiMonAn/GetDanhSachMonAn/{category}")]
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

            var data = query.Select(m => new
            {
                id = m.Id,
                tenMon = m.TenMon,
                moTa = m.MoTa,
                gia = m.Gia,
                image = m.Image,
                trangThai = m.TrangThai
            }).ToList();

            return Json(data);
        }

        [HttpPost]
        public IActionResult CapNhatTrangThai(int id, string trangThai, string returnUrl)
        {
            var mon = _context.Menu.FirstOrDefault(m => m.Id == id);
            if (mon != null && (trangThai == "Còn" || trangThai == "Đã Hết"))
            {
                mon.TrangThai = trangThai;
                _context.SaveChanges();
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }
    }
}
