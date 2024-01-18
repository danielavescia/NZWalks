using Microsoft.AspNet.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController( Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, ITokenRepository tokenRepository) 
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
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
                
                
                if ( checkPassword )
                {
                    var rolesUser = await userManager.GetRolesAsync( userLogin ); //get roles

                    if ( rolesUser != null ) {

                        var jwtToken= tokenRepository.CreateJWTToken( userLogin, rolesUser.ToList() ); //Create token
                        
                        var response = new LoginResponseDto
                        {
                            jwtToken = jwtToken
                        };

                        return Ok( response );
                    }
                }
            }

            return BadRequest( "Username or password incorrect!" );
        }
    }



}
