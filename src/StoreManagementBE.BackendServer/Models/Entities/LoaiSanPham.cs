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
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required (ErrorMessage = "Tên loại sản phẩm không được trống")]
        [Column("category_name", TypeName = "varchar(100")]
        public string CategoryName { get; set; } = "";
    }
}
