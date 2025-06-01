using System;
using System.ComponentModel.DataAnnotations;

namespace NhaHang.Models
{
    public class DatBan
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayDat { get; set; }
        public string GioNhanBan { get; set; }
        public string LoaiTiec { get; set; }
        public int SoNguoi { get; set; }
        public DateTime NgayTao { get; set; }
        public string TrangThai { get; set; } = "Chưa Đến";
    }
}