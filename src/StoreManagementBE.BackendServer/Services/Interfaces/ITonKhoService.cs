using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ITonKhoService
    {
        public Task<List<TonKhoDTO>> GetAll();
        public Task<TonKhoDTO> GetByProductID(int productID);
    }
}
