using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class ThanhToanDTO
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        [Column("amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        [Required]
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }
    }
}
