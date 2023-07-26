using HrSystem.DTO;
using HrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HrSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly UserManager<ApplicationUser> userManager;
        public readonly IConfiguration config;
        public AccountController(UserManager<ApplicationUser> userManager , IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        #region Create Account (Registeration)

        //[Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegisterDto userDto)
        {
            if (ModelState.IsValid)
            {
                
                ApplicationUser user = new ApplicationUser()
                {
                   
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    FullName = userDto.FullName,
                    Role = userDto.Role

                };
                     
                IdentityResult result=  await userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region Valid Account "Login" 
       
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid)
            {
              ApplicationUser user= await userManager.FindByEmailAsync(userDto.Email);
                if (user!= null)
                {
                bool checkPassword = await userManager.CheckPasswordAsync(user, userDto.Password); 
                    if (checkPassword)
                    {
                        // Claims Token

                        var claims = new List<Claim>();

                        //claims.Add(new Claim(ClaimTypes.Email, user.Email));
                        claims.Add(new Claim(ClaimTypes.Name, user.FullName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        claims.Add(new Claim(ClaimTypes.Role, user.Role));
                        //Get Roles
                        //var roles = await userManager.GetRolesAsync(user);
                        //foreach (var role in roles)
                        //{
                        //    claims.Add(new Claim(ClaimTypes.Role, role));
                        //}

                        //Secret Key
                        
                        SecurityKey securityKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));
                        SigningCredentials signingCred = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha512Signature);

                        // create Token (JSON Object)
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            //issuer: config["JWT:ValidIssuer"], //Url Api
                            //audience: config["JWT:ValidAudience"],  // URL Angular
                            claims: claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signingCred
                            );

                        // Token (String)
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expiration =myToken.ValidTo
                        });
                       
                    }

                }
                return Unauthorized();
            }

            return Unauthorized();
        }

  


        #endregion



    }
}
