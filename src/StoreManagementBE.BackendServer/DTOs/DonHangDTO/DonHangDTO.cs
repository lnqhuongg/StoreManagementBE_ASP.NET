using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class DonHangDTO
    {
        public int OrderId { get; set; }

        public int? CustomerId { get; set; }

        public int? UserId { get; set; }

        public int? PromoId { get; set; }

        public DateTime? OrderDate { get; set; }

        [Column("total_amount", TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        [Column("discount_amount", TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        public List<ChiTietDonHangDTO>? Items { get; set; }

        public List<ThanhToanDTO>? Payments { get; set; }
    }
}
