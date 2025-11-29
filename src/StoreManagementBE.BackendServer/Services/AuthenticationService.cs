using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.DTOs.AuthenticationDTO;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;
using System.ComponentModel;

namespace StoreManagementBE.BackendServer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthenticationService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NhanVienDTO?> Authenticate(AuthenticationDTO tk)
        {
            var username = tk.Username?.Trim();
            var password = tk.Password?.Trim();

            var user = await _context.NhanViens
                .FirstOrDefaultAsync(u => u.Username == username
                                       && u.Password == password);

            if (user == null)
                return null;

            return _mapper.Map<NhanVienDTO>(user);
        }

    }
}
