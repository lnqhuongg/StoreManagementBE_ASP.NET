using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class TonKhoDTO
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 0;
        public DateTime UpdatedAt { get; set; }
    }
}
