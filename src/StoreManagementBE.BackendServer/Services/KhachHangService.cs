using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly ApplicationDbContext _context;

        public KhachHangService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<KhachHang> GetAll()
        {
            return _context.KhachHangs.ToList();
        }

        public KhachHang? GetById(int customer_id)
        {
            return _context.KhachHangs.Find(customer_id);
        }

        public List<KhachHang> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _context.KhachHangs.ToList();
            return _context.KhachHangs
                .Where(x => x.name.Contains(keyword) || 
                            x.phone.Contains(keyword) || 
                            x.email.Contains(keyword))
                .ToList();
        }

        public bool Create(KhachHang kh)
        {
            try
            {
                _context.KhachHangs.Add(kh);
                _context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool Update(KhachHang kh)
        {
            var existing = _context.KhachHangs.Find(kh.customer_id);
            if (existing == null) return false;

            existing.name = kh.name;
            existing.phone = kh.phone;
            existing.email = kh.email;
            existing.address = kh.address;
            existing.reward_points = kh.reward_points;

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool IsExist(int customer_id)
        {
            return _context.KhachHangs.Any(x => x.customer_id == customer_id);
        }
    }
}
