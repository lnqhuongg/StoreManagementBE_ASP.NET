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
            return _context.LoaiSanPhams.Where(x => x.CategoryName.Contains(keyword)).ToList();
        }

        // tao moi 
        public async Task<LoaiSanPhamDTO> Create (LoaiSanPhamDTO loaiSanPhamDTO)
        {
            try
            {
                var loaiEntity = _mapper.Map<LoaiSanPham>(loaiSanPhamDTO);

                _context.LoaiSanPhams.Add(loaiEntity);

                await _context.SaveChangesAsync();

                return _mapper.Map<LoaiSanPhamDTO>(loaiEntity);
            }   
            catch(Exception ex)
            {
                throw new Exception("Lỗi khi thêm loại sản phẩm: " + ex.Message);
            }
        }

        public async Task<LoaiSanPhamDTO?> Update(int id, LoaiSanPhamDTO dto)
        {
            try
            {
                var existing = await _context.LoaiSanPhams.FindAsync(id);
                if (existing == null)
                {
                    return null; // ← 404
                }

                existing.CategoryName = dto.CategoryName;

                await _context.SaveChangesAsync();

                return _mapper.Map<LoaiSanPhamDTO>(existing);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật loại sản phẩm: " + ex.Message);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var existing = _context.LoaiSanPhams.Find(id);
                if (existing == null)
                {
                    return false;
                }

                _context.LoaiSanPhams.Remove(existing);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa loại sản phẩm: " + ex.Message);
            }
        }

        public async Task<bool> isCategoryNameExist(string category_name)
        {
            bool CategoryNameExist = await _context.LoaiSanPhams
                .AnyAsync(x => x.CategoryName == category_name);

            return CategoryNameExist;
        }

        public async Task<bool> isCategoryExist (int category_id)
        {
            bool CategoryExist = await _context.LoaiSanPhams
                .AnyAsync(x => x.CategoryId == category_id);
            return CategoryExist;
        }
    }
}
