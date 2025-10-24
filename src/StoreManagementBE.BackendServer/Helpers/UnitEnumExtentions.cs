using StoreManagementBE.BackendServer.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
namespace StoreManagementBE.BackendServer.Helpers
{
    public static class UnitEnumExtentions
    {
        public static string GetDisplayName(this UnitEnum value)
        {
            return value.GetType()
                .GetMember(value.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? value.ToString();
        }

    }
}
