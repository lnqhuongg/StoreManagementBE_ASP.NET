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

        // GET ALL
        public async Task<List<NhanVienDTO>> GetAll()
        {
            var list = await _context.NhanViens.ToListAsync();
            var dtos = _mapper.Map<List<NhanVienDTO>>(list);
            dtos.ForEach(dto => dto.Password = ""); // Không trả password
            return dtos;
        }

        // GET BY ID
        public async Task<NhanVienDTO?> GetById(int userId)
        {
            var entity = await _context.NhanViens.FindAsync(userId);
            if (entity == null) return null;

            var dto = _mapper.Map<NhanVienDTO>(entity);
            dto.Password = ""; // Không trả password
            return dto;
        }

        // SEARCH
        public List<NhanVienDTO> SearchByKeyword(string keyword)
        {
            IQueryable<NhanVien> query = _context.NhanViens;

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.Username.ToLower().Contains(keyword) ||
                    x.FullName.ToLower().Contains(keyword) ||
                    x.Role.ToLower().Contains(keyword));
            }

            var result = query.ToList();
            var dtos = _mapper.Map<List<NhanVienDTO>>(result);
            dtos.ForEach(dto => dto.Password = "");
            return dtos;
        }

        // CREATE
        public async Task<ApiResponse<NhanVienDTO>> Create(NhanVienDTO dto)
        {
            try
            {
                var exists = await _context.NhanViens.AnyAsync(x => x.Username == dto.Username);
                if (exists)
                {
                    return new ApiResponse<NhanVienDTO>
                    {
                        Success = false,
                        Message = "Tên đăng nhập đã tồn tại!"
                    };
                }

                var entity = _mapper.Map<NhanVien>(dto);
                entity.CreatedAt = DateTime.Now;
                entity.Status = 1;

                // TODO: Hash password
                // entity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                _context.NhanViens.Add(entity);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<NhanVienDTO>(entity);
                resultDto.Password = ""; // Không trả password

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
                    Message = ex.Message
                };
            }
        }

        // UPDATE
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

                // Kiểm tra username trùng (ngoại trừ chính nó)
                if (dto.Username != existing.Username && await IsUsernameExist(dto.Username))
                {
                    return new ApiResponse<NhanVienDTO>
                    {
                        Success = false,
                        Message = "Tên đăng nhập đã được sử dụng bởi tài khoản khác!"
                    };
                }

                // Cập nhật các trường
                existing.Username = dto.Username;
                existing.FullName = dto.FullName;
                existing.Role = dto.Role;
                existing.Status = dto.Status;
                // Không cập nhật Password

                _context.NhanViens.Update(existing);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<NhanVienDTO>(existing);
                resultDto.Password = "";

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
                    Message = ex.Message
                };
            }
        }

        // DELETE
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
                    Message = ex.Message,
                    DataDTO = false
                };
            }
        }

        // HELPER: Kiểm tra username tồn tại
        private async Task<bool> IsUsernameExist(string username)
        {
            return await _context.NhanViens.AnyAsync(x => x.Username == username);
        }
    }
}