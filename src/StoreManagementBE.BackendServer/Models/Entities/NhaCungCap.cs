using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("suppliers")]
    public class NhaCungCap
    {
        [Key]
        public int supplier_id { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc!")]
        [Column("name", TypeName = "varchar(100)")]
        public string name { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc!")]
        [Column("phone", TypeName = "varchar(20)")]
        [MaxLength(100, ErrorMessage = "Số điện thoại tối đa 20 kí tự!")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Email nhà cung cấp là bắt buộc!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        [Column("email", TypeName = "varchar(100)")]
        public string email { get; set; }

        [Column("address", TypeName = "text")]
        public string address { get; set; }

        [Column("status", TypeName = "bit(1)")]
        public int status { get; set; }

    }
}
