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

        public int UsernameChangeLimit { get; set; } = 10;

        public byte[] ProfilePicture { get; set; }
    }
}
