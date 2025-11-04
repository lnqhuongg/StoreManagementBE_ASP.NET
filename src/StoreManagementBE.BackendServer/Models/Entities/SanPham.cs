using StoreManagementBE.BackendServer.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("products")]
    public class SanPham
    {
        [Key]
        [Column("product_id")]
        public int ProductID { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public LoaiSanPham? Category { get; set; }

        [Column("supplier_id")]
        public int? SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        public NhaCungCap? Supplier { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc!")]
        [Column("product_name", TypeName = "varchar(100)")]
        [MaxLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 kí tự!")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Mã vạch là bắt buộc!")]
        [Column("barcode", TypeName = "varchar(50)")]
        [MaxLength(ErrorMessage = "Mã vạch tối đa 50 kí tự!")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc!")]
        [Column("price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column("unit", TypeName = "varchar(20)")]
        public string Unit { get; set; }

        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column("status", TypeName = "bit(1)")]
        public int Status { get; set; }
    }
}