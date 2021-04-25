using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuthorizationDemo.Constants;
using PermissionBasedAuthorizationDemo.Data.Seeds;
using PermissionBasedAuthorizationDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationDemo.Controllers
{
    [Authorize(Roles = nameof(Roles.SuperAdmin))]
    public class UserRolesController : Controller
    {
        private readonly SignInManager<IdentityUser> _singInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(
            SignInManager<IdentityUser> singInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _singInManager = singInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string userId)
        {
            var userRolesVMList = new List<UserRolesViewModel>();
            var user = await _userManager.FindByIdAsync(userId);

            foreach (var role in _roleManager.Roles)
            {
                var userRolesVM = new UserRolesViewModel
                {
                    RoleName = role.Name
                };

                userRolesVM.Selected = await _userManager.IsInRoleAsync(user, role.Name);

                userRolesVMList.Add(userRolesVM);
            }

            var manageUserRolesVM = new ManageUserRolesViewModel
            {
                UserId = userId,
                UserRoles = userRolesVMList
            };

            return View(manageUserRolesVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ManageUserRolesViewModel manageUserRolesVM)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            result = await _userManager.AddToRolesAsync(user, manageUserRolesVM.UserRoles.Where( x => x.Selected).Select(y => y.RoleName));

            //reapply roles and permissions for superAdmin in case any other admin tries to change its permissions
            //this can be changed or deleted depending on the business logic
            var currentUser = await _userManager.GetUserAsync(User);
            await _singInManager.RefreshSignInAsync(currentUser);
            await DefaultUsers.SeedSuperAdminAsync(_userManager, _roleManager);

            return RedirectToAction(nameof(Index), new { userId = id});

        }
    }
}
