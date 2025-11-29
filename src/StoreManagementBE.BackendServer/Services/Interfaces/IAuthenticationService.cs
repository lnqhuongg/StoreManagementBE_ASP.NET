using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.DTOs.AuthenticationDTO;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<NhanVienDTO?> Authenticate(AuthenticationDTO tk);
    }
}
