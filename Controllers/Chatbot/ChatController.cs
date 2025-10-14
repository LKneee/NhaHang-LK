using Microsoft.AspNetCore.Mvc;
using NhaHang.Data;
using NhaHang.Models;
using System.Linq;
using System.Collections.Generic;

namespace NhaHang.Controllers
{
    [Route("Chat")]
    public class ChatController : Controller
    {
        private readonly AppDbContext _db;

        public ChatController(AppDbContext db)
        {
            _db = db;
        }

        private static string RemoveVietnameseSigns(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (var ch in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC)
                .Replace("đ", "d")
                .Replace("Đ", "D");
        }

        private static readonly Dictionary<string, Func<IQueryable<Menu>, IQueryable<Menu>>> _keywordRules =
            new Dictionary<string, Func<IQueryable<Menu>, IQueryable<Menu>>>()
        {
            { "ruou vang do", menus => menus.Where(m => m.CategoryId == 10) },
            { "ruou do", menus => menus.Where(m => m.CategoryId == 10) },
            { "ruou vang trang", menus => menus.Where(m => m.CategoryId == 9) },
            { "ruou trang", menus => menus.Where(m => m.CategoryId == 9) },
            { "sinh to", menus => menus.Where(m => m.CategoryId == 8) },
            { "nuoc ep", menus => menus.Where(m => m.CategoryId == 7) },
            { "nuoc khoang", menus => menus.Where(m => m.TenMon.Contains("Nước Khoáng")) },
            { "bo", menus => menus.Where(m => m.CategoryId == 4) },
            { "ga", menus => menus.Where(m => m.CategoryId == 3) },
            { "hai san", menus => menus.Where(m => m.CategoryId == 2) },
            { "trang mieng", menus => menus.Where(m => m.CategoryId == 5) },
            { "nuoc", menus => menus.Where(m => m.CategoryId == 6) },
        };

        [HttpPost("Ask")]
        public IActionResult Ask([FromForm] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return Json(new { success = false, reply = "Xin lỗi, tôi không hiểu bạn nói gì." });

            string input = message.Trim().ToLower();
            string inputNoSign = RemoveVietnameseSigns(input);

            foreach (var rule in _keywordRules)
            {
                if (inputNoSign.Contains(rule.Key))
                {
                    var filtered = rule.Value(_db.Menu.Where(m => m.TrangThai == "Còn"));
                    var list = filtered.Select(m => new { m.TenMon, m.Gia, m.MoTa, m.Image }).ToList();
                    if (list.Any())
                        return Json(new { success = true, items = list });
                }
            }

            var allMenus = _db.Menu.Where(m => m.TrangThai == "Còn").ToList();
            string[] inputWords = inputNoSign.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var matched = allMenus.Where(m =>
            {
                string nameRaw = m.TenMon.ToLower();
                string nameNoSign = RemoveVietnameseSigns(nameRaw);

                if (inputNoSign == "ca")
                    return nameRaw.Split(' ').Any(w => w.StartsWith("cá"));

                if (inputNoSign == "bo")
                    return nameRaw.Contains("bò") || nameRaw.Contains("bơ");

                if (inputNoSign.Contains("thom"))
                    return nameRaw.Contains("thơm");

                if (inputNoSign.Contains("bong"))
                {
                    if (inputNoSign.Contains("he"))
                        return nameRaw.Contains("bông hẹ");
                    if (inputNoSign.Contains("thien ly"))
                        return nameRaw.Contains("bông thiên lý");
                    return nameRaw.Contains("bông");
                }

                if (inputNoSign.Contains("bap cai"))
                    return nameRaw.Contains("bắp cải");

                if (inputNoSign == "nuoc")
                    return m.CategoryId == 6;

                if (inputNoSign.Contains("nuoc khoang"))
                    return nameRaw.Contains("nước khoáng");

                if (inputNoSign.Contains("nuoc ep") || inputNoSign.Contains("ep"))
                    return nameRaw.Contains("ép");

                if (inputNoSign.Contains("hai san"))
                    return m.CategoryId == 2;

                if (inputNoSign.Contains("trang mieng"))
                    return m.CategoryId == 5;

                return inputWords.All(w => nameNoSign.Contains(w));
            }).ToList();

            if (!matched.Any())
            {
                return Json(new
                {
                    success = true,
                    reply = "Không tìm thấy món phù hợp."
                });
            }

            var result = matched.Select(x => new
            {
                x.TenMon,
                x.Gia,
                x.MoTa,
                x.Image
            }).ToList();

            return Json(new { success = true, items = result });
        }
    }
}
