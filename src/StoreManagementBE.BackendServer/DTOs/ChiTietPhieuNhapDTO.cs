namespace StoreManagementBE.BackendServer.DTOs
{
    public class ChiTietPhieuNhapDTO
    {
        public int Import_detail_id { get; set; }
        public int Import_id { get; set; }
        public int Product_id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
