using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class NhaCungCapService : INhaCungCapService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NhaCungCapService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Lấy tất cả
        public async Task<List<NhaCungCapDTO>> GetAll()
        {
            var list = await _context.NhaCungCaps.ToListAsync();
            return _mapper.Map<List<NhaCungCapDTO>>(list);  //return list DTO
        }

        // Lấy theo id
        public async Task<NhaCungCapDTO?> GetById(int supplierId)   //thêm dấu ? cho possible null return
        {
            //FindAsync(id):                            nhanh & tiện khi bạn có primary key; có thể trả về bản ghi đang tracked mà không hit DB.
            //FirstOrDefaultAsync(x => x.id == id):     dùng được mọi điều kiện (x.Email, x.Phone...), nhưng luôn query DB và không dùng cache theo khóa.
            var supplier = await _context.NhaCungCaps.FindAsync(supplierId);
            return supplier == null ? null : _mapper.Map<NhaCungCapDTO>(supplier);
        }

        // Tìm kiếm
        public List<NhaCungCap> SearchByKeyword(string keyword)
        {
            keyword = keyword?.Trim() ?? "";    //nếu null thì thành "", đồng thời Trim() bỏ khoảng trắng đầu/cuối

            //Tạo truy vấn LINQ trên DbSet<NhaCungCap>
            return _context.NhaCungCaps
                .Where(x => x.Name.Contains(keyword) ||        // true nếu Name chứa keyword
                            x.Email.Contains(keyword) ||       // true nếu Email chứa keyword
                            x.Phone.Contains(keyword) ||       // true nếu Phone chứa keyword
                            x.Address.Contains(keyword)        // true nếu Address chứa keyword
                        ).ToList();                            // thực thi truy vấn và trả về List<NhaCungCap>
        }

        // Tạo mới
        public async Task<ApiResponse<NhaCungCapDTO>> Create(NhaCungCapDTO nhaCungCapDTO)
        {
            try
            {
                if (await IsSupplierExist(nhaCungCapDTO.Name, nhaCungCapDTO.Email, nhaCungCapDTO.Phone))   //gọi service kiểm tra trùng
                {
                    return new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Nhà cung cấp đã tồn tại (trùng tên hoặc email hoặc sđt)!"
                    };
                }

                //Map DTO -> entity
                var entity = _mapper.Map<NhaCungCap>(nhaCungCapDTO);
                _context.NhaCungCaps.Add(entity);

                // Lưu vào DB
                await _context.SaveChangesAsync();

                var dataDTO = _mapper.Map<NhaCungCapDTO>(entity);
                return new ApiResponse<NhaCungCapDTO>
                {
                    Success = true,
                    Message = "Tạo mới nhà cung cấp thành công!",
                    DataDTO = dataDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<NhaCungCapDTO> { Success = false, Message = ex.Message };
            }
        }

        // Cập nhật
        public async Task<ApiResponse<NhaCungCapDTO>> Update(int id, NhaCungCapDTO nhaCungCapDTO)
        {
            try
            {
                //Kiểm tra đã tồn tại chưa
                var existed = await _context.NhaCungCaps.FirstOrDefaultAsync(x => x.SupplierId == id);
                if (existed == null)    //nếu không tìm thấy
                {
                    return new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Không tìm thấy nhà cung cấp!"
                    };
                }

                //kiểm tra trùng lắp
                if (await IsSupplierExist(nhaCungCapDTO.Name, nhaCungCapDTO.Email, nhaCungCapDTO.Phone, ignoreId: id))
                {
                    return new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Tên hoặc email hoặc sđt đã được dùng bởi nhà cung cấp khác!"
                    };
                }

                // Map DTO -> entity (giữ lại khóa)
                existed.Name = nhaCungCapDTO.Name;
                existed.Phone = nhaCungCapDTO.Phone;
                existed.Email = nhaCungCapDTO.Email;
                existed.Address = nhaCungCapDTO.Address;
                existed.Status = nhaCungCapDTO.Status;

                await _context.SaveChangesAsync();

                var dataDTO = _mapper.Map<NhaCungCapDTO>(existed);
                return new ApiResponse<NhaCungCapDTO>
                {
                    Success = true,
                    Message = "Cập nhật nhà cung cấp thành công!",
                    DataDTO = dataDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<NhaCungCapDTO> { Success = false, Message = ex.Message };
            }
        }

        // Xóa
        //public bool Delete(int id)
        //{
        //    try
        //    {
        //        var existing = _context.NhaCungCaps.Find(id);
        //        if (existing == null) return false;

        //        _context.NhaCungCaps.Remove(existing);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Lỗi khi xóa nhà cung cấp: " + ex.Message);
        //    }
        //}

        public async Task<bool> IsSupplierIdExist(int supplierId)
        {
            return await _context.NhaCungCaps.AnyAsync(x => x.SupplierId == supplierId);
        }

        public async Task<bool> IsSupplierExist(string name, string email, string phone, int? ignoreId = null)
        {
            return await _context.NhaCungCaps.AnyAsync(x =>
                (x.Name == name || x.Email == email || x.Phone == phone) &&
                (!ignoreId.HasValue || x.SupplierId != ignoreId.Value));
        }
    }
}
