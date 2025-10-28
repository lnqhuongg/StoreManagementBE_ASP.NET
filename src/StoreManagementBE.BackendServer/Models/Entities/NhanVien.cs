using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("users")]
    public class NhanVien
    {
        [Key]
        public int User_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Username { get; set; } = "";

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Password { get; set; } = "";

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Full_name { get; set; } = "";

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Role { get; set; } = "staff"; // admin, staff

        [Column(TypeName = "datetime")]
        public DateTime Created_at { get; set; } = DateTime.Now;

        [Column(TypeName = "tinyint")]
        public byte Status { get; set; } = 1; // 0: khóa, 1: hoạt động
    }
}