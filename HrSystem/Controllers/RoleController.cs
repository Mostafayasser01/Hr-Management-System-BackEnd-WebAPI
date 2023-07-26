using HrSystem.Constants;
using HrSystem.DTO;
using HrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrSystem.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private static List<Permission> permissions = new List<Permission>
        {
            new Permission { Id = 1, Name = "Permissions.Attendance.View"},
            new Permission { Id = 2, Name = "Permissions.Attendance.Create" },
            new Permission { Id = 3, Name = "Permissions.Attendance.Edit" },
            new Permission { Id = 4, Name = "Permissions.Attendance.Delete" },
            new Permission { Id = 5, Name = "Permissions.Employee.View"},
            new Permission { Id = 6, Name = "Permissions.Employee.Create" },
            new Permission { Id = 7, Name = "Permissions.Employee.Edit" },
            new Permission { Id = 8, Name = "Permissions.Employee.Delete" },
            new Permission { Id = 9, Name = "Permissions.GeneralSettings.View"},
            new Permission { Id = 10, Name = "Permissions.GeneralSettings.Create" },
            new Permission { Id = 11, Name = "Permissions.GeneralSettings.Edit" },
            new Permission { Id = 12, Name = "Permissions.GeneralSettings.Delete" },
            new Permission { Id = 13, Name = "Permissions.Holidays.View"},
            new Permission { Id = 14, Name = "Permissions.Holidays.Create" },
            new Permission { Id = 15, Name = "Permissions.Holidays.Edit" },
            new Permission { Id = 16, Name = "Permissions.Holidays.Delete" },
            new Permission { Id = 17, Name = "Permissions.Home.View"},
            new Permission { Id = 18, Name = "Permissions.Home.Create" },
            new Permission { Id = 19, Name = "Permissions.Home.Edit" },
            new Permission { Id = 20, Name = "Permissions.Home.Delete" },
            new Permission { Id = 21, Name = "Permissions.Roles.View"},
            new Permission { Id = 22, Name = "Permissions.Roles.Create" },
            new Permission { Id = 23, Name = "Permissions.Roles.Edit" },
            new Permission { Id = 24, Name = "Permissions.Roles.Delete" },
            new Permission { Id = 25, Name = "Permissions.SalaryReport.View"},
            new Permission { Id = 26, Name = "Permissions.SalaryReport.Create" },
            new Permission { Id = 27, Name = "Permissions.SalaryReport.Edit" },
            new Permission { Id = 28, Name = "Permissions.SalaryReport.Delete" },
            new Permission { Id = 29, Name = "Permissions.Users.View"},
            new Permission { Id = 30, Name = "Permissions.Users.Create" },
            new Permission { Id = 31, Name = "Permissions.Users.Edit" },
            new Permission { Id = 32, Name = "Permissions.Users.Delete" },

        };

        public readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        
        [HttpGet]
        [Route("api/permissions")]
        public ActionResult<IEnumerable<Permission>> GetPermissions()
        {
            return Ok(permissions);
        }

        #region GetAll Roles
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
        #endregion

        #region Add New Roles
        [HttpPost]

        public async Task<IActionResult> AddRole(RoleFormDto roleFormDto)
        {
            if (!ModelState.IsValid)
            {
                var roles = await roleManager.Roles.ToListAsync();
                return Ok(roles);
            }
            var roleExist = await roleManager.RoleExistsAsync(roleFormDto.Name);
            if (roleExist)
            {
                ModelState.AddModelError("Name", "Role Is Exist");
                return Content("Role Is Exist");
            }
            var newRole = await roleManager.CreateAsync(new IdentityRole(roleFormDto.Name.Trim()));
            return Ok(newRole);
        }

        #endregion

        [HttpGet("{roleId}")]
        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }
            var roleClaims = roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            var allClaims = Permissions.generateAllPemissions();
            var allPermissions = allClaims.Select(x => new CheckBoxDto { DisplayValue = x }).ToList();
            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(x => x == permission.DisplayValue))
                {
                    permission.IsSelected = true;
                }

            }
            var permissionRole = new PermissionFormDto
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleClaims = allPermissions

            };
            return Ok(permissionRole);
        }

    }
}
