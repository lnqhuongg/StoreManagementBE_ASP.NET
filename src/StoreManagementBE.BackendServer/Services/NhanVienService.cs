using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagementBE.BackendServer.Services
{
    public class NhanVienService : INhanVienService
    {
        private readonly ApplicationDbContext _context;

        public NhanVienService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<NhanVien> GetAll()
        {
            return _context.NhanViens.ToList();
        }

        public NhanVien GetById(int user_id)
        {
            return _context.NhanViens.Find(user_id);
        }

        public List<NhanVien> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _context.NhanViens.ToList();

            keyword = keyword.ToLower();
            return _context.NhanViens
                .Where(x => x.username.ToLower().Contains(keyword) ||
                            x.full_name.ToLower().Contains(keyword) ||
                            x.role.ToLower().Contains(keyword))
                .ToList();
        }

        public bool Create(NhanVien nhanVien)
        {
            if (IsUsernameExist(nhanVien.username))
                return false;

            try
            {
                _context.NhanViens.Add(nhanVien);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(NhanVien nhanVien)
        {
            var existing = _context.NhanViens.Find(nhanVien.user_id);
            if (existing == null) return false;

            if (IsUsernameExist(nhanVien.username, nhanVien.user_id))
                return false;

            existing.username = nhanVien.username;
            existing.full_name = nhanVien.full_name;
            existing.role = nhanVien.role;
            existing.status = nhanVien.status;

            // Không cập nhật password ở đây (nếu cần thì dùng API riêng)
            // existing.password = nhanVien.password;

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool IsExist(int user_id)
        {
            return _context.NhanViens.Find(user_id) != null;
        }

        public bool IsUsernameExist(string username, int? excludeId = null)
        {
            return _context.NhanViens
                .Any(x => x.username == username && (excludeId == null || x.user_id != excludeId));
        }
    }
}