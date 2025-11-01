using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class LoaiSanPhamService : ILoaiSanPhamService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LoaiSanPhamService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // lay tat ca loai san pham
        public async Task<List<LoaiSanPhamDTO>> GetAll()
        {
            var list = await _context.LoaiSanPhams.ToListAsync();
            // tra ve danh sach DTO
            return _mapper.Map<List<LoaiSanPhamDTO>>(list);
        }

        // lay ra 1 san pham theo id 
        public async Task<LoaiSanPhamDTO> GetById(int category_id)
        {
            var category = await _context.LoaiSanPhams.FindAsync(category_id);
            if (category == null) return null;
            else return _mapper.Map<LoaiSanPhamDTO>(category);
        }

        // tim kiem loai san pham
        public List<LoaiSanPham> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _context.LoaiSanPhams.ToList();
            return _context.LoaiSanPhams.Where(x => x.Category_name.Contains(keyword)).ToList();
        }

        // tao moi 
        public async Task<ApiResponse<LoaiSanPhamDTO>> Create (LoaiSanPhamDTO loaiSanPhamDTO)
        {
            try
            {
                var exists = await _context.LoaiSanPhams.AnyAsync(x => x.Category_name == loaiSanPhamDTO.Category_name);
                if (exists)
                {
                    return new ApiResponse<LoaiSanPhamDTO>
                    {
                        Success = false,
                        Message = "Loại sản phẩm đã tồn tại!"
                    };
                }

                var loaiEntity = _mapper.Map<LoaiSanPham>(loaiSanPhamDTO);
                _context.LoaiSanPhams.Add(loaiEntity);
                await _context.SaveChangesAsync();

                var dataDTO = _mapper.Map<LoaiSanPhamDTO>(loaiEntity);
                return new ApiResponse<LoaiSanPhamDTO>
                {
                    Success = true,
                    Message = "Thêm loại sản phẩm mới thành công!",
                    DataDTO = dataDTO
                };
            }   
            catch(Exception ex)
            {
                return new ApiResponse<LoaiSanPhamDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<LoaiSanPhamDTO>> Update(int id, LoaiSanPhamDTO loaiSanPhamDTO)
        {
            try
            {
                var existing = await _context.LoaiSanPhams.FindAsync(id);
                if (existing == null)
                {
                    return new ApiResponse<LoaiSanPhamDTO>
                    {
                        Success = false,
                        Message = "Không tìm thấy loại sản phẩm để cập nhật!"
                    };
                }

                // Cập nhật dữ liệu
                existing.Category_name = loaiSanPhamDTO.Category_name;

                _context.LoaiSanPhams.Update(existing);
                await _context.SaveChangesAsync();

                var dataDTO = _mapper.Map<LoaiSanPhamDTO>(existing);
                return new ApiResponse<LoaiSanPhamDTO>
                {
                    Success = true,
                    Message = "Cập nhật loại sản phẩm thành công!",
                    DataDTO = dataDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoaiSanPhamDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<bool>> Delete(int id)
        {
            try
            {
                var existing = await _context.LoaiSanPhams.FindAsync(id);
                if (existing == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy loại sản phẩm để xóa!",
                        DataDTO = false
                    };
                }

                _context.LoaiSanPhams.Remove(existing);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Xóa loại sản phẩm thành công!",
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
    }
}
