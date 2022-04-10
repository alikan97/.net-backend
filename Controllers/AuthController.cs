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
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            return users;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var user = await _userService.GetUser(id);
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create (UserDto user)
        {
            var newUser = await _userService.Create(user);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login ([FromBody] UserDto user)
        {
            var token = await _userService.Authenticate(user);

            if (token == null) return Unauthorized();

            return Ok(new {token, user});
        }
    }
}
