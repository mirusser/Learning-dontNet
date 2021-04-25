using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementDemo.Models;
using UserManagementDemo.Models.DbModels;

namespace UserManagementDemo.Controllers
{
    [Authorize(Roles = nameof(Models.Enums.Roles.SuperAdmin))]
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleManagerController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var roleExists = (await _roleManager.FindByNameAsync(roleName)) != null;

            if (!string.IsNullOrEmpty(roleName) && !roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
