using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class TonKhoService : ITonKhoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TonKhoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TonKhoDTO>> GetAll()
        {
            var list = await _context.TonKhos.ToListAsync();
            return _mapper.Map<List<TonKhoDTO>>(list);
        }
        public async Task<TonKhoDTO> GetByProductID(int productID)
        {
            try
            {
                var tonkho = await _context.TonKhos.FirstOrDefaultAsync(x => x.ProductId == productID);
                return _mapper.Map<TonKhoDTO>(tonkho);
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
