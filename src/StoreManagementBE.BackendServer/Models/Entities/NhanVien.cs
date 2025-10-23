using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("users")]
    public class NhanVien
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string username { get; set; } = "";

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string password { get; set; } = "";

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string full_name { get; set; } = "";

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string role { get; set; } = "staff"; // admin, staff

        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; } = DateTime.Now;

        [Column(TypeName = "tinyint")]
        public byte status { get; set; } = 1; // 0: khóa, 1: hoạt động
    }
}