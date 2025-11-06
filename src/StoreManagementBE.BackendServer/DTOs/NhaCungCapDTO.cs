namespace StoreManagementBE.BackendServer.DTOs
{
    public class NhaCungCapDTO
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Status { get; set; } = 0;
<<<<<<<<< Temporary merge branch 1
        public int Status { get; set; }
        public NhaCungCapDTO() { }
        public NhaCungCapDTO(int supplier_id, string name, string phone, string email, string address, int status)
        {
            this.Supplier_id = supplier_id;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Address = address;
            this.Status = status;
        }
=========
        
>>>>>>>>> Temporary merge branch 2
    }
}