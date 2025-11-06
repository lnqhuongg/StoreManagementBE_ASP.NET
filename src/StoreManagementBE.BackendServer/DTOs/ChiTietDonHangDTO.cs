using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class ChiTietDonHangDTO
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column("subtotal", TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
    }
}
