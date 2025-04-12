using Microsoft.AspNetCore.Mvc;

namespace NhaHang.Controllers.QuanLy
{
    public class QuanLyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
