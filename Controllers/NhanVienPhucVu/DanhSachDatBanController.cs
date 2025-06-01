using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;

namespace NhaHang.Controllers.NhanVienPhucVu
{
    public class DanhSachDatBanController : Controller
    {
        private readonly AppDbContext _context;

        public DanhSachDatBanController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.DatBan.OrderByDescending(d => d.NgayTao).ToList();
            return View("~/Views/NhanVienPhucVu/DanhSachDatBan/Index.cshtml", list);
        }
    }
}
