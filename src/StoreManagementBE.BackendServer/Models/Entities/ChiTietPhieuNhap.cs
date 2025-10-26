using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("import_details")]
    public class ChiTietPhieuNhap
    {
        [Key]
        public int import_detail_id { get; set; }

        [Required]
        public int import_id { get; set; }

        [Required]
        public int product_id { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal subtotal { get; set; }

        [JsonIgnore]
        [ForeignKey("import_id")]
        public PhieuNhap Import { get; set; }

    }
}
