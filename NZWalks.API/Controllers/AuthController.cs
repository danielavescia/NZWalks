using Microsoft.AspNet.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;

        public AuthController( Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager) 
        {
            this.userManager = userManager;
        }

        //Register
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

        //Login
        // /api/Auth/Login
        [HttpPost]
        [Route( "Login" )]
        public async Task<IActionResult> Login( [FromBody] LoginRequestDto loginDto )
        {
            var userLogin = await userManager.FindByEmailAsync( loginDto.Username );


            if ( userLogin != null ) 
            {
                var  checkPassword = await userManager.CheckPasswordAsync( userLogin, loginDto.Password );

                //Create token
                if ( checkPassword )
                {
                    return Ok();
                }
            }

            return BadRequest( "Username or password incorrect!" );
        }
    }



}
