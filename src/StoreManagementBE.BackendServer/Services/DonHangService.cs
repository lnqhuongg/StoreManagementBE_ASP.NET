using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class DonHangService : IDonHangService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DonHangService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DonHangDTO>> GetAll()
        {
            var list = await _context.DonHangs
                .AsNoTracking()
                .Include(d => d.Items)
                .Include(d => d.Payments)
                .ToListAsync();

            return _mapper.Map<List<DonHangDTO>>(list);
        }

        public async Task<DonHangDTO?> GetById(int id)
        {
            var entity = await _context.DonHangs
                .AsNoTracking()
                .Include(d => d.Items)
                .Include(d => d.Payments)
                .FirstOrDefaultAsync(d => d.OrderId == id);

            return entity == null ? null : _mapper.Map<DonHangDTO>(entity);
        }
    }
}
