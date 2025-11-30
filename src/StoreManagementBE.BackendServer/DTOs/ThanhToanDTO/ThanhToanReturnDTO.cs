using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.DTOs.ThanhToanDTO
{
    public class ThanhToanReturnDTO
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
    }
}
