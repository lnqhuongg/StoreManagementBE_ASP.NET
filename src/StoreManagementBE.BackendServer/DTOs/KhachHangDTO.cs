namespace StoreManagementBE.BackendServer.DTOs
{
    public class KhachHangDTO
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int RewardPoints { get; set; }
    }
}