using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NhaHang.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HoTen { get; set; }

        public string GioiTinh { get; set; }

        [Required]
        public string SDT { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MatKhau { get; set; } 

        public string DiaChi { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string VaiTro { get; private set; }

    }
}
