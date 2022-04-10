using Microsoft.AspNetCore.Mvc;
using System;
using Server.Dtos;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Models;
using MongoDB.Driver;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/configure")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userMgr, RoleManager<ApplicationRole> roleMgr)
        {
            _userManager = userMgr;
            _roleManager = roleMgr;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole ([FromBody] CreateRoleDto role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role.roleName);
            if (!roleExists)
            {
                var newRole = new ApplicationRole(role.roleName);
                // newRole.Claims.Add(new AspNetCore.Identity.MongoDbCore.Models.MongoClaim )
                var roleResult = await _roleManager.CreateAsync(newRole);
                if (roleResult.Succeeded) {
                    var result = await _roleManager.FindByNameAsync(role.roleName);
                    return Ok(result);
                } else {
                    return BadRequest(new {error = "Role was not added"});
                }
            }
            return BadRequest(new {error="Role already exists"});
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetAllUsers ()
        {
            var users = _userManager.Users.ToString();
            return Ok(users);
        }

        [HttpPost]
        [Route("add-to-role")]
        public async Task<IActionResult> AddUserToRole ([FromBody] UserRoleDto role) {
            var user = await _userManager.FindByEmailAsync(role.Email);
            if (user == null) return BadRequest(new { error = "User does not exist"});

            var roleExists = await _roleManager.RoleExistsAsync(role.roleName);
            if (!roleExists) return BadRequest(new { error = "Role does not exist"});

            var result = await _userManager.AddToRoleAsync(user, role.roleName);
            if ( result.Succeeded ) return Ok (new { result = "success", user = user.ToString() });
            return BadRequest(new {error = user});
        }

        [HttpGet]
        [Route("get-user-roles")]
        public async Task<IActionResult> GetUserRoles ([FromBody] UserRoleDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser == null) return BadRequest(new {error = "Error: User does not exist"});

            var roles = await _userManager.GetRolesAsync(existingUser);
            return Ok(roles);
        }

        [HttpPost]
        [Route("remove-user-from-role")]
        public async Task<IActionResult> DeleteUserFromRole ([FromBody] UserRoleDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser == null) return BadRequest(new {error = "Error: User does not exist"});

            var isRemoved = await _userManager.RemoveFromRoleAsync(existingUser, user.roleName);
            if (isRemoved.Succeeded) return Ok($"User was removed from Role: {user.roleName}");
            else return BadRequest(new {error = "User was not removed"});
        }
    }
}
