using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NhaHang.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Column(TypeName = "VARCHAR(20)")]
        public string Ban { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string NhanVienHoTen { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string NhanVienEmail { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string OrderType { get; set; }  // Khách hàng hay Nhân viên

        [Column(TypeName = "NVARCHAR(500)")]
        public string GhiChu { get; set; }

        [Column(TypeName = "DECIMAL(18, 2)")]
        public decimal TongTien { get; set; }

        public DateTime NgayDat { get; set; } = DateTime.Now;
        public string TrangThai { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string ThanhToan { get; set; } = "Chưa thanh toán";

        // Quan hệ 1-n
        public List<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
    }
}
