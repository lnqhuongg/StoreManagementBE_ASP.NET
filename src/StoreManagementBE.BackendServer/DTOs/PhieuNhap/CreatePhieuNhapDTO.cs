using StoreManagementBE.BackendServer.DTOs.ChiTietPhieuNhap;

namespace StoreManagementBE.BackendServer.DTOs.PhieuNhap
{
    public class CreatePhieuNhapDTO // đơn giản hóa json thêm phiếu nhập, xử lí các thuộc tính còn lại của PhieuNhapDTO ở service
    {
        public int SupplierId { get; set; }
        public int UserId { get; set; }
        public List<CreateChiTietPNDTO>? ImportDetails { get; set; }
    }
}
