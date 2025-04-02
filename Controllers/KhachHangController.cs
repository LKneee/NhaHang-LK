using Microsoft.AspNetCore.Mvc;

namespace NhaHang.Controllers
{
    public class KhachHangController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
