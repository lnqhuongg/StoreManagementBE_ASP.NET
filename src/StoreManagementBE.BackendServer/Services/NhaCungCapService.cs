using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        // Lấy tất cả nhà cung cấp
        public async Task<List<NhaCungCapDTO>> GetAll()
        {
            var list = await _context.NhaCungCaps.AsNoTracking().ToListAsync();
            return _mapper.Map<List<NhaCungCapDTO>>(list);
        }

        // Lấy 1 nhà cung cấp theo ID
        public async Task<NhaCungCapDTO> GetById(int supplier_id)
        {
            var supplier = await _context.NhaCungCaps.FindAsync(supplier_id);
            //FindAsync(id):                            nhanh & tiện khi bạn có primary key; có thể trả về bản ghi đang tracked mà không hit DB.
            //FirstOrDefaultAsync(x => x.id == id):     dùng được mọi điều kiện (x.Email, x.Phone...), nhưng luôn query DB và không dùng cache theo khóa.

            if (supplier == null) return null;
            else return _mapper.Map<NhaCungCapDTO>(supplier);
        }

        public List<NhaCungCap> SearchByKeyword(string keyword)
        {
            //nếu null thì thành "", đồng thời Trim() bỏ khoảng trắng đầu/cuối
            keyword = keyword?.Trim() ?? "";

            //Tạo truy vấn LINQ trên DbSet<NhaCungCap>
            return _context.NhaCungCaps
                     .Where(x =>
                            x.Name.Contains(keyword) ||        // true nếu Name chứa keyword
                            x.Email.Contains(keyword) ||       // true nếu Email chứa keyword
                            x.Phone.Contains(keyword) ||       // true nếu Phone chứa keyword
                            x.Address.Contains(keyword)        // true nếu Address chứa keyword
                        ).ToList();                            // thực thi truy vấn và trả về List<NhaCungCap>
        }

        //Create từ hư vô
        public async Task<ApiResponse<NhaCungCapDTO>> Create(NhaCungCapDTO nhaCungCapDTO)
        {
            try
            {
                //Kiểm tra trùng lắp theo tên + email + sdt (tùy bạn chỉnh rule)
                var existed = await _context.NhaCungCaps
                    .AnyAsync(x => x.Name == nhaCungCapDTO.Name
                                || x.Email == nhaCungCapDTO.Email
                                || x.Phone == nhaCungCapDTO.Phone);
                if (existed)
                {
                    return new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Nhà cung cấp đã tồn tại (trùng tên hoặc email hoặc sđt)!"
                    };
                }

                //Map DTO -> entity
                var nccEntity = _mapper.Map<NhaCungCap>(nhaCungCapDTO);
                _context.NhaCungCaps.Add(nccEntity);

                //Lưu vào DB
                await _context.SaveChangesAsync();

                var dataDTO = _mapper.Map<NhaCungCapDTO>(nccEntity);
                return new ApiResponse<NhaCungCapDTO>
                {
                    Success = true,
                    Message = "Tạo mới nhà cung cấp thành công!",
                    DataDTO = dataDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<NhaCungCapDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<NhaCungCapDTO>> Update(int id, NhaCungCapDTO nhaCungCapDTO)
        {
            try
            {
                //Kiểm tra đã tồn tại chưa
                var existed = await _context.NhaCungCaps.FirstOrDefaultAsync(x => x.SupplierId == id);
                if (existed == null)
                {
                    return new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Không tìm thấy nhà cung cấp!"
                    };
                }

                // Check trùng 
                var duplicated = await _context.NhaCungCaps
                    .AnyAsync(x => (x.Name == nhaCungCapDTO.Name
                                || x.Email == nhaCungCapDTO.Email
                                || x.Phone == nhaCungCapDTO.Phone) && x.SupplierId != id);
                if (duplicated)
                {
                    return new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Tên hoặc email đã được dùng bởi nhà cung cấp khác!"
                    };
                }

                // Map DTO -> entity (giữ lại khóa)
                existed.Name = nhaCungCapDTO.Name;
                existed.Phone = nhaCungCapDTO.Phone;
                existed.Email = nhaCungCapDTO.Email;
                existed.Address = nhaCungCapDTO.Address;
                existed.Status = nhaCungCapDTO.Status == 1;

                //Luu vào DB
                _context.NhaCungCaps.Update(existed);
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
                return new ApiResponse<NhaCungCapDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
