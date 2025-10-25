using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("suppliers")]
    public class NhaCungCap
    {
        [Key] // khóa chính
        public int supplier_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string name { get; set; } = "";

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string phone { get; set; } = "";

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string email { get; set; } = "";

        [Required]
        [Column(TypeName = "text")]
        public string address { get; set; } = "";

        [Required]
        [Column(TypeName = "bit(1)")]
        public bool status { get; set; }
    }
}
