using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/auth")]
    [ApiController]
    public class AuthMgmtController : ControllerBase
    {
        private readonly UserService _userService;
        public AuthMgmtController(UserService service)
        {
            _userService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<filteredUser>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            var filtered = new List<filteredUser> {};
            
            users.ForEach(x => {
                filtered.Add(
                    new filteredUser {
                        Email = x.Email,
                        Id = x.Id,
                        Roles = x.Roles
                    }
                );
            });

            return filtered;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<filteredUser>> GetUser(Guid id)
        {
            var user = await _userService.GetUser(id);
            return new filteredUser {
                Email = user.Email,
                Id = user.Id,
                Roles = user.Roles
            };
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create ([FromBody] userRegistrationDto user)
        {
            var newUser = await _userService.Create(user);
            if (newUser == null) return BadRequest(new {error = "Error occurred during creation"});
            return Ok(newUser.Id);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login ([FromBody] UserLoginDto user)
        {
            if (!ModelState.IsValid) return BadRequest("Check your input");
            var token = await _userService.Authenticate(user);

            if (token == null) return Unauthorized();

            var filteredUser = new filteredUser { Email = user.Email };
            return Ok(new {token, filteredUser});
        }
    }
}
