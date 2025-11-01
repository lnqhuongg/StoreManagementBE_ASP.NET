// BackendServer/DTOs/NhanVienDTO.cs
using System.ComponentModel.DataAnnotations;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class NhanVienDTO
    {
        public int User_id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = "";

        // Không trả về password
        [Required]
        [StringLength(100)]
        public string Full_name { get; set; } = "";

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "staff"; // admin, staff

        public DateTime Created_at { get; set; }

        public byte Status { get; set; } = 1; // 0: khóa, 1: hoạt động

        public NhanVienDTO() { }

        public NhanVienDTO(int user_id, string username, string full_name, string role, DateTime created_at, byte status)
        {
            User_id = user_id;
            Username = username;
            Full_name = full_name;
            Role = role;
            Created_at = created_at;
            Status = status;
        }
    }
}