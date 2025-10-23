using StoreManagementBE.BackendServer.Models.Entities;
using System.Collections.Generic;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface INhanVienService
    {
        List<NhanVien> GetAll();
        NhanVien GetById(int user_id);
        List<NhanVien> SearchByKeyword(string keyword);
        bool Create(NhanVien nhanVien);
        bool Update(NhanVien nhanVien);
        bool IsExist(int user_id);
        bool IsUsernameExist(string username, int? excludeId = null);
    }
}