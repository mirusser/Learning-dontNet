using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuthorizationDemo.Constants;
using PermissionBasedAuthorizationDemo.Helpers;
using PermissionBasedAuthorizationDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationDemo.Controllers
{
    [Authorize(Roles = nameof(Roles.SuperAdmin))]
    public class PermissionController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ActionResult> Index(string roleId)
        {
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Products), roleId);

            var role = await _roleManager.FindByIdAsync(roleId);

            var claims = await _roleManager.GetClaimsAsync(role);
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }

            var permissionVM = new PermissionViewModel
            {
                RoleId = roleId,
                RoleClaims = allPermissions
            };

            return View(permissionVM);
        }

        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();

            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }

            return RedirectToAction(nameof(Index), new { roleId = model.RoleId });
        }
    }
}
