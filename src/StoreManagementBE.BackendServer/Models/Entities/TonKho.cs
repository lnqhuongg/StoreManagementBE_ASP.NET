using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementBE.BackendServer.Models.Entities
{
    [Table("inventory")]
    public class TonKho
    {
        [Key]
        public int Inventory_id { get; set; }

        [Required]
        public int Product_id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime Update_at { get; set; }
    }
}
