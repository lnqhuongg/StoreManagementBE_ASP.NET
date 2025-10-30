namespace StoreManagementBE.BackendServer.DTOs
{
    public class ChiTietPhieuNhapDTO
    {
        public int ImportDetailId { get; set; }
        public int ImportId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}