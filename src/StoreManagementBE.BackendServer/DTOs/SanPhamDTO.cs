using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.DTOs
{
    public class SanPhamDTO
    {
        public int product_id { get; set; }
        public LoaiSanPhamDTO Category { get; set; }
        public NhaCungCapDTO Supplier { get; set; }
        public string product_name { get; set; }
        public string barcode { get; set; }
        public decimal price { get; set; }
        public string unit { get; set; }
        public DateTime created_at { get; set; }
        public int status { get; set; }

    }
}