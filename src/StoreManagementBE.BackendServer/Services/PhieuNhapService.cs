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

        //get all
        public async Task<List<PhieuNhapDTO>> GetAll()
        {
            var list = await _context.PhieuNhaps
                //.Include(p => p.Staff)         // nạp luôn thông tin nhân viên
                .Include(p => p.Supplier)
                .Include(p => p.ImportDetails)
                .ToListAsync();
            return _mapper.Map<List<PhieuNhapDTO>>(list);
        }

        //get by id
        public async Task<PhieuNhapDTO> GetById(int id)
        {
            if (!isExist(id)) return null;
            var phieuNhap = await _context.PhieuNhaps
                //.Include(p => p.Staff)
                .Include(p => p.Supplier)
                .Include(p => p.ImportDetails)
                .FirstOrDefaultAsync(p => p.ImportId == id);
            return _mapper.Map<PhieuNhapDTO>(phieuNhap);
        }


        //create
        public async Task<PhieuNhapDTO> Create(PhieuNhapDTO phieuNhapDto)
        {
            try
            {
                var phieuNhap = _mapper.Map<PhieuNhap>(phieuNhapDto);
                _context.PhieuNhaps.Add(phieuNhap);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    // Nạp thêm navigation properties (nếu cần)
                    await _context.Entry(phieuNhap).Reference(p => p.Supplier).LoadAsync();
                    await _context.Entry(phieuNhap).Collection(p => p.ImportDetails).LoadAsync();

                    return _mapper.Map<PhieuNhapDTO>(phieuNhap);
                }
                else
                {
                    Console.WriteLine("Thêm phiếu nhập thất bại: Không có bản ghi nào được thêm.");
                    return null; // thêm thất bại
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm phiếu nhập: {ex.Message}");
                return null;
            }
        }

        //update
        public async Task<PhieuNhapDTO> Update(PhieuNhapDTO phieuNhapDto)
        {
            var phieuNhapTMP = await _context.PhieuNhaps.FindAsync(phieuNhapDto.ImportId);
            if (phieuNhapTMP == null) return null;
            else
            {
                phieuNhapTMP.ImportDate = phieuNhapDto.ImportDate;
                phieuNhapTMP.SupplierId = phieuNhapDto.Supplier.supplier_id;
                phieuNhapTMP.UserId = phieuNhapDto.UserId; // sua thanh User va User.UserId
                phieuNhapTMP.TotalAmount = phieuNhapDto.TotalAmount;
                try
                {
                    _context.PhieuNhaps.Update(phieuNhapTMP);
                    var rs = await _context.SaveChangesAsync();
                    if (rs > 0)
                    {
                        return _mapper.Map<PhieuNhapDTO>(phieuNhapTMP);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Lỗi khi cập nhật phiếu nhập: {ex.Message}");
                    return null;
                }
            }
        }

        //delete
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
            return _context.PhieuNhaps.Any(e => e.ImportId == id);
        }

        //search by keyword
        public List<PhieuNhapDTO> SearchByKeyword(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
