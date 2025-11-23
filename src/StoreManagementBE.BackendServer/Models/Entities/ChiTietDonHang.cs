using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("order_items")]
    public class ChiTietDonHang
    {
        [Key]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("product_id")]
        public int? ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        // --- MỐI QUAN HỆ VỚI ĐƠN HÀNG (Đã có) ---
        [ForeignKey(nameof(OrderId))]
        [JsonIgnore]
        public DonHang? Order { get; set; }

        // --- 👇 BỔ SUNG PHẦN NÀY 👇 ---
        // Liên kết sang bảng SanPham để lấy tên sản phẩm
        // Lưu ý: Class SanPham phải tồn tại trong project của bạn
        [ForeignKey(nameof(ProductId))]
        public SanPham? Product { get; set; }
    }
}