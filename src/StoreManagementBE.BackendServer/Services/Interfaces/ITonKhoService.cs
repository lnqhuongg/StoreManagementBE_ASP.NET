using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ITonKhoService
    {
        Task<List<TonKhoDTO>> GetAll();
        Task<TonKhoDTO> GetByProductID(int productID);
        Task<TonKhoDTO> deductQuantityOfCreatedOrder(int productID, int quantityChange);
    }
}
