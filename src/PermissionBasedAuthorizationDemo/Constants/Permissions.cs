using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationDemo.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"{nameof(Permissions)}.{module}.Create",
                $"{nameof(Permissions)}.{module}.View",
                $"{nameof(Permissions)}.{module}.Edit",
                $"{nameof(Permissions)}.{module}.Delete",
            };
        }

        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }
    }
}
