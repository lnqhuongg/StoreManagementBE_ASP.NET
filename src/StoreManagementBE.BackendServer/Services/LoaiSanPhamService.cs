using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class LoaiSanPhamService : ILoaiSanPhamService
    {
        private readonly ApplicationDbContext _context;

        public LoaiSanPhamService(ApplicationDbContext context)
        {
            _context = context;
        }

        // lay tat ca loai san pham
        public List<LoaiSanPham> GetAll()
        {
            return _context.LoaiSanPhams.ToList();
        }

        public LoaiSanPham GetById(int category_id)
        {
            return _context.LoaiSanPhams.Find(category_id);
        }

        public List<LoaiSanPham> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _context.LoaiSanPhams.ToList();
            return _context.LoaiSanPhams.Where(x => x.category_name.Contains(keyword)).ToList();
        }

        public bool Create(LoaiSanPham loai)
        {
            try
            {
                _context.LoaiSanPhams.Add(loai);
                _context.SaveChanges();
                return true; // thêm thành công
            }
            catch
            {
                return false; // thêm thất bại
            }
        }

        public bool Update(LoaiSanPham loai)
        {
            var loaiTMP = _context.LoaiSanPhams.Find(loai.category_id);
            if (loaiTMP == null) return false;

            if (isExist(loai.category_id))
            {
                loaiTMP.category_name = loai.category_name;
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
            else return false;
        }

        public bool Delete(int category_id)
        {
            LoaiSanPham loaiTMP = _context.LoaiSanPhams.Find(category_id);
            if (isExist(category_id))
            {
                _context.LoaiSanPhams.Remove(loaiTMP);

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
            else return false;
        }

        public bool isExist(int category_id)
        {
            var existing = _context.LoaiSanPhams.Find(category_id);
            if (existing == null)
                return false;
            else return true;
        }
    }
}
