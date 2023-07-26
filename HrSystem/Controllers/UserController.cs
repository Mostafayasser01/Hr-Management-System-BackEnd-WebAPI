using HrSystem.Constants;
using HrSystem.DTO;
using HrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static HrSystem.Constants.Permissions;

namespace HrSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITIContext context;

        public UserController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ITIContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<ApplicationUser> users = context.Users.ToList();
            List<UserDto> userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                UserDto userDto = new UserDto
                {
                    ID = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Password = user.PasswordHash,
                    Role = (List<string>)_userManager.GetRolesAsync(user).Result
                };
                userDtos.Add(userDto);
            }


            return Ok(userDtos);
        }


    }
}
