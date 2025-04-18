namespace NhaHang.Models
{
    public class DonHang
    {
        public string banSo { get; set; }
        public string ghiChu { get; set; }
        public string orderType { get; set; }
        public List<Mon> phieu { get; set; }
    }

    public class Mon
    {
        public string ten { get; set; }
        public int gia { get; set; }
        public int soluong { get; set; }
    }
}
