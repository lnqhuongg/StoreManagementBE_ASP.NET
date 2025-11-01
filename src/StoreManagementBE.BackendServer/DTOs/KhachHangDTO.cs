namespace StoreManagementBE.BackendServer.DTOs
{
    public class KhachHangDTO
    {
        public int Customer_id { get; set; }
        public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? Created_at { get; set; }
        public int Reward_points { get; set; }

        public KhachHangDTO() { }

        public KhachHangDTO(int customer_id, string name, string? phone, string? email, string? address, DateTime? created_at, int reward_points)
        {
            this.Customer_id = customer_id;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Address = address;
            this.Created_at = created_at;
            this.Reward_points = reward_points;
        }
    }
}