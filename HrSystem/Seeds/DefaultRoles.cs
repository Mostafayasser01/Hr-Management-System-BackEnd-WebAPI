using HrSystem.Constants;
using Microsoft.AspNetCore.Identity;

namespace HrSystem.Seeds
{
    public static class DefaultRoles
    {
        public static async Task seedAsync(RoleManager<IdentityRole> roleManager )
        {
            // Awl mara hyft7 lapplication el Role De hht-create
            // Lw m3ndesh ay Roles F Application Add Role De
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }
        }
    }
}
