namespace StoreManagementBE.BackendServer.DTOs
{
    public class LoaiSanPhamDTO
    {
        public int category_id { get; set; }
        public string category_name { get; set; } = "";
        public LoaiSanPhamDTO(int category_id, string category_name)
        {
            this.category_id = category_id;
            this.category_name = category_name;
        }
        public LoaiSanPhamDTO() { }
    }
}
