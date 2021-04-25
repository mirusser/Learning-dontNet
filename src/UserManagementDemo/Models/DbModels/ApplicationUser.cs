using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementDemo.Models.DbModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DefaultValue(10)]
        public int UsernameChangeLimit { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}
