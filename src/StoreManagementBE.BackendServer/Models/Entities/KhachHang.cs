using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("customers")]
    public class KhachHang
    {
        [Key]
        public int Customer_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = "";

        [Column(TypeName = "varchar(20)")]
        public string? Phone { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime? Created_at { get; set; } = DateTime.Now;

        public int Reward_points { get; set; } = 0; 
    }
}