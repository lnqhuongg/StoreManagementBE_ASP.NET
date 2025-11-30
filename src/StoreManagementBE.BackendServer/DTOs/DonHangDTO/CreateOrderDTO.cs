using StoreManagementBE.BackendServer.DTOs.ThanhToanDTO;

namespace StoreManagementBE.BackendServer.DTOs.DonHangDTO
{
    public class CreateOrderDTO
    {
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public int? PromoId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<ChiTietDonHangDTO>? Items { get; set; }
        public List<CreateThanhToanDTO>? Payments { get; set; }

        public int? rewardPoints { get; set; }
    }
}
