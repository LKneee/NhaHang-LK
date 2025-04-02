using Microsoft.AspNetCore.Mvc;

namespace NhaHang.Controllers
{
    public class QuanLyController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
