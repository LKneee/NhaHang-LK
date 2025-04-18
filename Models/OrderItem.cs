using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NhaHang.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; } // Khóa ngoại

        public string TenMon { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        // THÊM DÒNG NÀY để ánh xạ ngược về Order
        public Order Order { get; set; }
    }
}
