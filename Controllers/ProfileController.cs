using Microsoft.AspNetCore.Mvc;
using NhaHang.Models;
using System.IO;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NhaHang.Data;

namespace NhaHang.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

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
                GioiTinh = HttpContext.Session.GetString("UserGioiTinh"),
                Image = HttpContext.Session.GetString("UserImage")
            };

            return View(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            if (image != null && image.Length > 0)
            {
               
                if (!string.IsNullOrEmpty(user.Image))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatar", fileName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn ảnh trong DB và session
                user.Image = "/avatar/" + fileName;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("UserImage", user.Image);
            }

            return RedirectToAction("Index");
        }
    }
}
