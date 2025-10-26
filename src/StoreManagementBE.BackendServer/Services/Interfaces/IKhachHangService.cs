using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IKhachHangService
    {
        List<KhachHang> GetAll();
        KhachHang? GetById(int customer_id);
        List<KhachHang> SearchByKeyword(string keyword);
        bool Create(KhachHang kh);
        bool Update(KhachHang kh);
        bool IsExist(int customer_id);
    }
}
