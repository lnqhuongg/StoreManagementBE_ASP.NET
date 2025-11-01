// Services/NhanVienService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class NhanVienService : INhanVienService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NhanVienService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NhanVienDTO>> GetAll()
        {
            var list = await _context.NhanViens.ToListAsync();
            return _mapper.Map<List<NhanVienDTO>>(list);
        }

        public async Task<NhanVienDTO?> GetById(int user_id)
        {
            var entity = await _context.NhanViens.FindAsync(user_id);
            return entity == null ? null : _mapper.Map<NhanVienDTO>(entity);
        }

        public List<NhanVienDTO> SearchByKeyword(string keyword)
        {
            IQueryable<NhanVien> query = _context.NhanViens;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(x =>
                    x.Username.ToLower().Contains(keyword) ||
                    x.Full_name.ToLower().Contains(keyword) ||
                    x.Role.ToLower().Contains(keyword));
            }

            var result = query.ToList();
            return _mapper.Map<List<NhanVienDTO>>(result);
        }

        public async Task<ApiResponse<NhanVienDTO>> Create(NhanVienDTO dto)
        {
            try
            {
                if (await IsUsernameExist(dto.Username))
                {
                    return new ApiResponse<NhanVienDTO>
                    {
                        Success = false,
                        Message = "Tên đăng nhập đã tồn tại!"
                    };
                }

                var entity = _mapper.Map<NhanVien>(dto);
                entity.Created_at = DateTime.Now;
                entity.Status = 1;

                // TODO: Mã hóa mật khẩu trước khi lưu
                // entity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                _context.NhanViens.Add(entity);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<NhanVienDTO>(entity);
                return new ApiResponse<NhanVienDTO>
                {
                    Success = true,
                    Message = "Thêm nhân viên thành công!",
                    DataDTO = resultDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<NhanVienDTO>
                {
                    Success = false,
                    Message = "Lỗi khi thêm nhân viên: " + ex.Message
                };
            }
        }

        public async Task<ApiResponse<NhanVienDTO>> Update(int id, NhanVienDTO dto)
        {
            try
            {
                var existing = await _context.NhanViens.FindAsync(id);
                if (existing == null)
                {
                    return new ApiResponse<NhanVienDTO>
                    {
                        Success = false,
                        Message = "Không tìm thấy nhân viên để cập nhật!"
                    };
                }

                if (await IsUsernameExist(dto.Username, id))
                {
                    return new ApiResponse<NhanVienDTO>
                    {
                        Success = false,
                        Message = "Tên đăng nhập đã được sử dụng bởi tài khoản khác!"
                    };
                }

                // Cập nhật các trường
                existing.Username = dto.Username;
                existing.Full_name = dto.Full_name;
                existing.Role = dto.Role;
                existing.Status = dto.Status;

                // Không cập nhật password ở đây
                // existing.Created_at giữ nguyên

                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<NhanVienDTO>(existing);
                return new ApiResponse<NhanVienDTO>
                {
                    Success = true,
                    Message = "Cập nhật nhân viên thành công!",
                    DataDTO = resultDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<NhanVienDTO>
                {
                    Success = false,
                    Message = "Lỗi khi cập nhật: " + ex.Message
                };
            }
        }

        public async Task<ApiResponse<bool>> Delete(int id)
        {
            try
            {
                var existing = await _context.NhanViens.FindAsync(id);
                if (existing == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy nhân viên để xóa!",
                        DataDTO = false
                    };
                }

                _context.NhanViens.Remove(existing);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Xóa nhân viên thành công!",
                    DataDTO = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Lỗi khi xóa: " + ex.Message,
                    DataDTO = false
                };
            }
        }

        public async Task<bool> IsUsernameExist(string username, int? excludeId = null)
        {
            return await _context.NhanViens
                .AnyAsync(x => x.Username == username && (excludeId == null || x.User_id != excludeId));
        }
    }   
}