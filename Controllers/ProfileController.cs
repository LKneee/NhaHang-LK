using Microsoft.AspNetCore.Mvc;

namespace NhaHang.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
  
            if (HttpContext.Session.GetString("UserEmail") == null)
            {

                return RedirectToAction("Login", "Auth");
            }

           
            var userInfo = new
            {
                HoTen = HttpContext.Session.GetString("UserName"),
                SDT = HttpContext.Session.GetString("UserSDT"),
                Email = HttpContext.Session.GetString("UserEmail"),
                DiaChi = HttpContext.Session.GetString("UserDiaChi"),
                GioiTinh = HttpContext.Session.GetString("UserGioiTinh")
            };

            return View(userInfo);
        }
    }
}

