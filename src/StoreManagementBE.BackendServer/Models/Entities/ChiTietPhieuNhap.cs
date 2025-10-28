using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("import_details")]
    public class ChiTietPhieuNhap
    {
        [Key]
        public int Import_detail_id { get; set; }

        [Required]
        public int Import_id { get; set; }

        [Required]
        public int Product_id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        [JsonIgnore]
        [ForeignKey("import_id")]
        public PhieuNhap Import { get; set; }

    }
}