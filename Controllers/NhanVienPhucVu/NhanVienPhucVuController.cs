using Microsoft.AspNetCore.Mvc;

namespace NhaHang.Controllers.NhanVienPhucVu
{
    public class NhanVienPhucVuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return RedirectToAction("Index", "MenuNvpv");
        }

    }
}
