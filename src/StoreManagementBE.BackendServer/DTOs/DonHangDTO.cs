using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class DonHangDTO
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }= 0;
        public int? UserId { get; set; }= 0;
        public int? PromoId { get; set; }= 0;
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }= 0;
        public decimal DiscountAmount { get; set; }= 0;
        public List<ChiTietDonHangDTO> Items { get; set; }
        public List<ThanhToanDTO> Payments { get; set; }
    }
}
