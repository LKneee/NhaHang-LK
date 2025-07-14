using System;
using System.ComponentModel.DataAnnotations;

namespace NhaHang.Models
{
    public class DatBan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng điền đầy đủ họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày đặt")]
        public DateTime NgayDat { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giờ nhận bàn")]
        public string GioNhanBan { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại tiệc")]
        public string LoaiTiec { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số người")]
        [Range(1, 100, ErrorMessage = "Số người phải từ 1 đến 100")]
        public int? SoNguoi { get; set; }

        public DateTime NgayTao { get; set; }

        public string TrangThai { get; set; } = "Chưa Đến";
    }
}