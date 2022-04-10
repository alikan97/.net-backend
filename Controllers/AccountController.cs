using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.Dtos;
using Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Server.Config;
using Microsoft.IdentityModel.Tokens;
using Server.Repositories;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using AspNetCore.Identity.Mongo;
using App.Models;
using System.Linq;

namespace Server.Controllers
{
    [Authorize]
    [Route("api/auth")]
    public class AuthMgmtController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParam;

        public AuthMgmtController(UserManager<ApplicationUser> userManager, TokenValidationParameters tokenValidParam, RoleManager<ApplicationRole> roleMgr, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _roleManager = roleMgr;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParam = tokenValidParam;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<String>() {
                            "User already exists"
                        },
                        Success = false,
                    });
                }

                var newUser = new ApplicationUser() { Email = user.Email, UserName = user.Email };
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {
                    return Ok(new AuthResult()
                    {
                        Errors = null,
                        Success = true,
                        Token = null
                    });
                }
                else
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<String>() {
                            "Error occurred during user creation"
                        },
                        Success = false,
                    });
                }
            }
            return BadRequest(new AuthResult()
            {
                Errors = new List<String>() {
                            "Error processing request"
                        },
                Success = false,
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto newUser)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(newUser.Email);
                if (existingUser == null) return BadRequest("User doesn't exist");

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, newUser.Password);
                if (!isCorrect) return BadRequest("Incorrect Password");
                var jwtToken = await GenerateJwtTokens(existingUser);

                return Ok(jwtToken.Token);
            }
            return BadRequest(new AuthResult()
            {
                Token = null,
                Success = true,
                refreshToken = null,
                Errors = new List<string> {"Bad Request darl"}
            });
        }

        private async Task<AuthResult> GenerateJwtTokens(ApplicationUser user)
        {
            var idToken = new IdentityUserToken<string>();

            return new AuthResult()
            {
                Token = idToken.ToString(),
                Success = true,
                refreshToken = null
            };
        }

        private async Task<Boolean> verifyToken(ApplicationUser user, IdentityUserToken<string> token)
        {
            try
            {
                var isLegit = await _userManager.VerifyUserTokenAsync(user, token.LoginProvider, "Login", token.ToString());
                if (isLegit == true) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
