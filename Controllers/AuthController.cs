using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Server.Repositories;
using Server.Entities;

namespace Server.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthMgmtController : ControllerBase
    {
        private readonly UserService _userService;
        public AuthMgmtController(UserService service)
        {
            _userService = service;
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet("{email}")]
        public async Task<ActionResult<filteredUser>> GetUser(string email)
        {
            var user = await _userService.GetUser(email);
            return new filteredUser {
                Email = user.Email,
                Id = user.Id,
                Roles = user.Roles
            };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<Guid>> Create ([FromBody] userRegistrationDto user)
        {
            if (!ModelState.IsValid) return BadRequest("Bad Request");
            
            var existingUser = await _userService.GetUser(user.Email);
            if (existingUser != null) return BadRequest("User already exists");

            var newUser = await _userService.Create(user);
            if (newUser == null) return BadRequest(new {error = "Error occurred during creation"});
            return Ok(new {newUser.Roles, newUser.Id});
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login ([FromBody] UserLoginDto user)
        {
            if (!ModelState.IsValid) return BadRequest("Check your input");
            var token = await _userService.Authenticate(user);

            if (token == null) return Unauthorized();

            var filteredUser = new filteredUser { Email = user.Email, Info = "Don't pay attention to role or id, everythings all g"};
            return Ok(new LoginResponse 
            {
                statusCode = 200,
                ErrorContent = null,
                Token = token,
                Success = true,
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("add-role-to-user")]
        public async Task<ActionResult> addRoleToUser ([FromBody] addRoleToUser role)
        {
            if (!ModelState.IsValid) return BadRequest(new GenericResponse{ ErrorContent = "Bad Request", statusCode = 404});

            var user = await _userService.GetUser(role.Email);
            await _userService.AddRoleToUser(user, role.Role);
            return Ok($"{role.Role} role was added to User {role.Email}");
        }
    }
}
