using StoreManagementBE.BackendServer.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("products")]
    public class SanPham
    {
        [Key]
        public int product_id { get; set; }

        [Column("category_id")]
        public int? category_id { get; set; }
        [ForeignKey(nameof(category_id))]
        public LoaiSanPham? Category { get; set; }

        [Column("supplier_id")]
        public int? supplier_id { get; set; }
        [ForeignKey(nameof(supplier_id))]
        public NhaCungCap? Supplier { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc!")]
        [Column("product_name", TypeName = "varchar(100)")]
        [MaxLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 kí tự!")]
        public string product_name { get; set; }

        [Required(ErrorMessage = "Mã vạch là bắt buộc!")]
        [Column("barcode", TypeName = "varchar(50)")]
        [MaxLength(ErrorMessage = "Mã vạch tối đa 50 kí tự!")]
        public string barcode { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc!")]
        [Column("price", TypeName = "decimal(10,2)")]
        public decimal price { get; set; }

        [Column("unit", TypeName = "varchar(20)")]
        public UnitEnum unit { get; set; }

        [Column("created_at", TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column("status", TypeName = "bit(1)")]
        public int status { get; set; }

        public SanPham(int product_id, int? category_id, LoaiSanPham? category, int? supplier_id, NhaCungCap? supplier, string product_name, string barcode, decimal price, UnitEnum unit, DateTime created_at, int status)
        {
            this.product_id = product_id;
            this.category_id = category_id;
            Category = category;
            this.supplier_id = supplier_id;
            Supplier = supplier;
            this.product_name = product_name;
            this.barcode = barcode;
            this.price = price;
            this.unit = unit;
            this.created_at = created_at;
            this.status = status;
        }
        public SanPham() { }
    }
}
