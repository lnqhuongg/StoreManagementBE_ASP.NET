namespace StoreManagementBE.BackendServer.DTOs
{
    public class NhaCungCapDTO
    {
        public int supplier_id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public int status { get; set; }
        public NhaCungCapDTO() { }
        public NhaCungCapDTO(int supplier_id, string name, string phone, string email, string address, int status)
        {
            this.supplier_id = supplier_id;
            this.name = name;
            this.phone = phone;
            this.email = email;
            this.address = address;
            this.status = status;
        }
    }
}