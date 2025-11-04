using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class SanPhamDTO
    {
        public int ProductID { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
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