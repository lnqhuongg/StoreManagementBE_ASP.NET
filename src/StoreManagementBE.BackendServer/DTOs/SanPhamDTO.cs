using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class SanPhamDTO
    {
        public int ProductId { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public LoaiSanPhamDTO Category { get; set; }
        public NhaCungCapDTO Supplier { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
    }
}