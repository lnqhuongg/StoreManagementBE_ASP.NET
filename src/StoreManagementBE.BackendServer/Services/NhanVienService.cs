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
            var dtos = _mapper.Map<List<NhanVienDTO>>(list);
            dtos.ForEach(dto => dto.Password = "");
            return dtos;
        }

        public async Task<NhanVienDTO?> GetById(int userId)
        {
            var entity = await _context.NhanViens.FindAsync(userId);
            if (entity == null) return null;

            var dto = _mapper.Map<NhanVienDTO>(entity);
            dto.Password = "";
            return dto;
        }

        public List<NhanVien> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _context.NhanViens.ToList();

            return _context.NhanViens.Where(x =>
                x.Username.Contains(keyword) ||
                x.FullName.Contains(keyword) ||
                x.Role.Contains(keyword)).ToList();
        }

        // CREATE: Không cho phép trùng Username
        public async Task<NhanVienDTO> Create(NhanVienDTO dto)
        {
            // Kiểm tra trùng username
            if (await isUsernameExist(dto.Username))
            {
                throw new Exception("Tên đăng nhập đã tồn tại!");
            }

            var entity = _mapper.Map<NhanVien>(dto);
            entity.CreatedAt = DateTime.Now;
            entity.Status = 1;

            // TODO: Hash password
            // entity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            _context.NhanViens.Add(entity);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<NhanVienDTO>(entity);
            resultDto.Password = "";
            return resultDto;
        }

        // UPDATE: Không cho phép trùng Username (ngoại trừ chính nó)
        public async Task<NhanVienDTO?> Update(int id, NhanVienDTO dto)
        {
            var existing = await _context.NhanViens.FindAsync(id);
            if (existing == null)
            {
                return null; // 404
            }

            // Kiểm tra: Nếu username thay đổi → kiểm tra trùng
            if (dto.Username != existing.Username && await isUsernameExist(dto.Username))
            {
                throw new Exception("Tên đăng nhập đã được sử dụng bởi tài khoản khác!");
            }

            existing.Username = dto.Username;
            existing.FullName = dto.FullName;
            existing.Role = dto.Role;
            existing.Status = dto.Status;

            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<NhanVienDTO>(existing);
            resultDto.Password = "";
            return resultDto;
        }

        // Kiểm tra username tồn tại
        public async Task<bool> isUsernameExist(string username)
        {
            return await _context.NhanViens.AnyAsync(x => x.Username == username);
        }

        // Kiểm tra user tồn tại theo ID
        public async Task<bool> isUserExist(int userId)
        {
            return await _context.NhanViens.AnyAsync(x => x.UserId == userId);
        }
    }
}