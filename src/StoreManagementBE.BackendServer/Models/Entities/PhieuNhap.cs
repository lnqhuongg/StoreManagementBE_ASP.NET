using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("imports")]
    public class PhieuNhap
    {
        [Key]
        public int Import_id { get; set; }

        [Required]
        public DateTime Import_date { get; set; }

        [Required]
        public int Supplier_id { get; set; }

        [Required]
        public int User_id { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total_amount { get; set; }

        [ForeignKey("user_id")]
        public NhanVien? Staff { get; set; }

        [ForeignKey("supplier_id")]
        public NhaCungCap? Supplier { get; set; }

        public ICollection<ChiTietPhieuNhap>? ImportDetails { get; set; }
    }
}