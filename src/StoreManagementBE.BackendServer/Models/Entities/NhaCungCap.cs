using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("suppliers")]
    public class NhaCungCap
    {
        [Key] // khóa chính
        public int Supplier_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = "";

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; } = "";

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; } = "";

        [Required]
        [Column(TypeName = "text")]
        public string Address { get; set; } = "";

        [Required]
        [Column(TypeName = "bit(1)")]
        public bool Status { get; set; }
    }
}