using Microsoft.AspNetCore.Mvc;

namespace NhaHang.Controllers.QuanLy
{
    public class QuanLyController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "DonHangQuanLy");
        }
    }
}
