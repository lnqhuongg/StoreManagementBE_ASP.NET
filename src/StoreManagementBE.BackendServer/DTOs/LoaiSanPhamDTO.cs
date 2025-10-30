
namespace StoreManagementBE.BackendServer.DTOs
{
    public class LoaiSanPhamDTO
    {
        public int Category_id { get; set; }
        public string Category_name { get; set; } = "";
        public LoaiSanPhamDTO(int category_id, string category_name)
        {
            this.Category_id = category_id;
            this.Category_name = category_name;
        }
        public LoaiSanPhamDTO() { }
    }
}
