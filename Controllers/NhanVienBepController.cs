using Microsoft.AspNetCore.Mvc;

namespace NhaHang.Controllers
{
    public class NhanVienBepController : Controller
    {
        public IActionResult KitchenView()
        {
            return View();
        }
    }
}
