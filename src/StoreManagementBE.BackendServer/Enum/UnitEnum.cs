using System.ComponentModel.DataAnnotations;

namespace StoreManagementBE.BackendServer.Enum
{
    public enum UnitEnum
    {
        [Display(Name = "hộp")]
        HOP,

        [Display(Name = "cái")]
        CAI,

        [Display(Name = "tuýp")]
        TUYP,

        [Display(Name = "lon")]
        LON,

        [Display(Name = "chai")]
        CHAI,

        [Display(Name = "gói")]
        GOI
    }
}
