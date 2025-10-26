using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("customers")]
    public class KhachHang
    {
        [Key]
        public int customer_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string name { get; set; } = "";

        [Column(TypeName = "varchar(20)")]
        public string? phone { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? email { get; set; }

        public string? address { get; set; }

        public DateTime? created_at { get; set; } = DateTime.Now;

        public int reward_points { get; set; } = 0;
    }
}
