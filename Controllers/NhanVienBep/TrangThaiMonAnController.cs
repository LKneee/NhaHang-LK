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
