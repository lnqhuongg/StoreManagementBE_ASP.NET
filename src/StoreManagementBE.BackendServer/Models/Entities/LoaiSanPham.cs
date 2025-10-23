using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("categories")]
    public class LoaiSanPham
    {
        [Key] // khóa chính
        public int category_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100")]
        public string category_name { get; set; } = "";
    }
}
