using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class PhieuNhapDTO
    {
        public int Import_id { get; set; }
        public DateTime Import_date { get; set; }
        public int Supplier_id { get; set; } // dung NhanVienDTO
        public int Staff_id { get; set; } // dung NhaCungCapDTO
        public decimal Total_amount { get; set; }

        public List<ChiTietPhieuNhapDTO>? ImportDetails { get; set; }
    }
}
