
using System.ComponentModel.DataAnnotations;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class PhieuNhapDTO
    {
        public int ImportId { get; set; }
        public DateTime ImportDate { get; set; }

        [Required(ErrorMessage = "Nhà cung cấp không được trống")] //DataAnnotations
        public NhaCungCapDTO Supplier { get; set; } // dung NhaCungCapDTO

        [Required(ErrorMessage = "Nhân viên không được trống")]
        public int UserId { get; set; } // dung NhanVienDTO
        public decimal TotalAmount { get; set; }

        public List<ChiTietPhieuNhapDTO>? ImportDetails { get; set; }
    }
}