namespace NhaHang.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string TenMon { get; set; }
        public string MoTa { get; set; }
        public string Image { get; set; }
        public decimal Gia { get; set; }
        public int CategoryId { get; set; }
        public int? GiamGia { get; set; }
        public DateTime? SpecialDay { get; set; }
        public int? Combo { get; set; }
        public string TrangThai { get; set; } = "Còn"; 

    }
}
