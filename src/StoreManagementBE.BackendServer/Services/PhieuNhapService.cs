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
                .Include(p => p.Staff)       
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
                .Include(p => p.Staff)
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

                // Nếu có supplier id
                if (phieuNhap.Supplier != null && phieuNhap.Supplier.SupplierId > 0)
                {
                    // Lấy entity thực từ context để gắn vào
                    var existingSupplier = await _context.NhaCungCaps.FindAsync(phieuNhap.Supplier.SupplierId);
                    if (existingSupplier != null)
                    {
                        phieuNhap.Supplier = existingSupplier; // Gắn lại entity đang được track
                        phieuNhap.SupplierId = existingSupplier.SupplierId;
                    }
                }
                if (phieuNhap.Staff != null && phieuNhap.Staff.UserId > 0)
                {
                    var existingStaff = await _context.NhanViens.FindAsync(phieuNhap.Staff.UserId);
                    if (existingStaff != null)
                    {
                        phieuNhap.Staff = existingStaff;
                        phieuNhap.UserId = existingStaff.UserId;
                    }
                }

                _context.PhieuNhaps.Add(phieuNhap);

                var result = _context.SaveChanges(acceptAllChangesOnSuccess: false);
                _context.ChangeTracker.AcceptAllChanges();

                _context.ChangeTracker.DetectChanges();

                if (result > 0)
                {
                    // Nạp thêm navigation properties (nếu cần)
                    await _context.Entry(phieuNhap).Reference(p => p.Supplier).LoadAsync();
                    await _context.Entry(phieuNhap).Reference(p => p.Staff).LoadAsync();
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

        public async Task<PhieuNhapDTO> Update(PhieuNhapDTO phieuNhapDto)
        {
            try
            {
                var phieuNhap = await _context.PhieuNhaps
                    .Include(p => p.Supplier)
                    .Include(p => p.Staff)
                    .FirstOrDefaultAsync(p => p.ImportId == phieuNhapDto.ImportId);

                if (phieuNhap == null)
                {
                    Console.WriteLine("Không tìm thấy phiếu nhập để cập nhật.");
                    return null;
                }

                // Gán lại các thuộc tính cơ bản
                phieuNhap.ImportDate = phieuNhapDto.ImportDate;
                phieuNhap.TotalAmount = phieuNhapDto.TotalAmount;

                // Gán lại SupplierId (nếu có)
                if (phieuNhapDto.Supplier != null)
                {
                    phieuNhap.SupplierId = phieuNhapDto.Supplier.SupplierId;
                }

                // Không gán lại Supplier object để tránh lỗi tracked entity
                phieuNhap.Supplier = null;

                if (phieuNhapDto.Staff != null)
                {
                    phieuNhap.UserId = phieuNhapDto.Staff.UserId;
                }
                // Không gán lại Staff object để tránh lỗi tracked entity
                phieuNhap.Staff = null;

                // Đánh dấu entity là modified
                _context.Entry(phieuNhap).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    // Nạp lại navigation property sau khi cập nhật
                    await _context.Entry(phieuNhap).Reference(p => p.Supplier).LoadAsync();
                    return _mapper.Map<PhieuNhapDTO>(phieuNhap);
                }
                else
                {
                    Console.WriteLine("Không có thay đổi nào được lưu.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật phiếu nhập: {ex.Message}");
                return null;
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
