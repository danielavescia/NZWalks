using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        public AuthController(UserManager<IdentityUser> userManager) 
        {
            this.userManager = userManager;
        }
        // /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register( [FromBody] RegisterRequestDto registerDto) 
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Username
            };

            var identity = await userManager.CreateAsync( identityUser, registerDto.Password );

            //Add roles
            if ( identity.Succeeded ) 
            {
               
                    if ( registerDto.Roles != null && registerDto.Roles.Any() )
                    {
                    foreach ( var role in registerDto.Roles )
                    {
                        identity = await userManager.AddToRoleAsync( identityUser, role );

                        if ( identity.Succeeded )
                        {
                            return Ok( "User was registered! Now, please login!" );
                        }
                    }
                }
            } 
            return BadRequest("Something went wrong");
        }
    }
}
