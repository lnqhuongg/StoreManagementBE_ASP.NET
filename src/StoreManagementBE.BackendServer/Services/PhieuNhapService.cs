using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;
using AutoMapper;

namespace StoreManagementBE.BackendServer.Services
{
    public class PhieuNhapService : IPhieuNhapService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PhieuNhapService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PhieuNhapDTO>> GetAll()
        {
            var list = await _context.PhieuNhaps
                //.Include(p => p.Staff)         // nạp luôn thông tin nhân viên
                //.Include(p => p.Supplier)      
                .Include(p => p.ImportDetails)
                .ToListAsync();
            return _mapper.Map<List<PhieuNhapDTO>>(list);
        }

        public async Task<PhieuNhapDTO> GetById(int id)
        {
            if (!isExist(id)) return null;
            var phieuNhap = await _context.PhieuNhaps
                //.Include(p => p.Staff)
                //.Include(p => p.Supplier)
                .Include(p => p.ImportDetails)
                .FirstOrDefaultAsync(p => p.import_id == id);
            return _mapper.Map<PhieuNhapDTO>(phieuNhap);
        }

        public bool Create(PhieuNhap phieuNhap)
        {
            try
            {
                _context.PhieuNhaps.Add(phieuNhap);
                _context.SaveChanges();
                return true; // thêm thành công
            }
            catch
            {
                return false; // thêm thất bại
            }
        }
        public bool Update(PhieuNhap phieuNhap)
        {
            var phieuNhapTMP = _context.PhieuNhaps.Find(phieuNhap.import_id);
            if (phieuNhapTMP == null) return false;
            if (isExist(phieuNhap.import_id))
            {
                phieuNhapTMP.import_date = phieuNhap.import_date;
                phieuNhapTMP.supplier_id = phieuNhap.supplier_id;
                phieuNhapTMP.user_id = phieuNhap.user_id;
                phieuNhapTMP.total_amount = phieuNhap.total_amount;
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
        public bool Delete(int id)
        {
            var phieuNhapTMP = _context.PhieuNhaps.Find(id);
            if (phieuNhapTMP == null) return false;
            if (isExist(id))
            {
                try
                {
                    _context.PhieuNhaps.Remove(phieuNhapTMP);
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
        public bool isExist(int id)
        {
            return _context.PhieuNhaps.Any(e => e.import_id == id);
        }

        public List<PhieuNhap> SearchByKeyword(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
