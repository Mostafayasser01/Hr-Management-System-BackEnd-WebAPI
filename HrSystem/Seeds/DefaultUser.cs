using HrSystem.Constants;
using HrSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HrSystem.Seeds
{
    public static class DefaultUser
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            var DefaultUser = new ApplicationUser
            {
                FullName="Mariam Zaher Fahmy",
                UserName= "MaZaher",
                Email ="mz@gmail.com",
                EmailConfirmed= true,
            };
           var user= await userManager.FindByEmailAsync(DefaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(DefaultUser, "MaZaher23/11");
                await userManager.AddToRoleAsync(DefaultUser, Roles.Admin.ToString());

            }
            //seed Claims

            await roleManager.SeedClaimsForAdmin();

        }
   
        public static async Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
            {
                var adminRole = await roleManager.FindByNameAsync("Admin");
                await roleManager.addPermissionsclaims(adminRole, "Roles");
                await roleManager.addPermissionsclaims(adminRole, "Users");
                await roleManager.addPermissionsclaims(adminRole, "Employees");
                await roleManager.addPermissionsclaims(adminRole, "GeneralSettings");
                await roleManager.addPermissionsclaims(adminRole, "Holidays");
                await roleManager.addPermissionsclaims(adminRole, "Attendance");
                await roleManager.addPermissionsclaims(adminRole, "SalaryReport");
                await roleManager.addPermissionsclaims(adminRole, "Home");
              

            }

        public static async Task addPermissionsclaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var permissions = Permissions.generatePermissionsList(module);
            foreach (var permission in permissions)
            {
                if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }


    }
}
