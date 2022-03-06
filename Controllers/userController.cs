using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Entities;
using Server.Utilities;
using System;
using Server.Dtos;
using System.Threading.Tasks;
using System.Net;

namespace Server.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repository;

        public UserController(IUserRepository repo)
        {
            this.repository = repo;
        }

        // POST items
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> RegisterUser (UserDto userDto) { // Conventional to return the created Dto on successful POST
            User existingUser = await repository.GetUserAsync(userDto);
            if (existingUser != null) {
                return StatusCode(409, "User already exists with that email!");
            }

            var hash = SecureHasher.Hash(userDto.Password);
            
            User user = new() {
                Id = Guid.NewGuid(),
                Email = userDto.Email,
                Password = hash,
            };

            await repository.Register(user);
            return StatusCode(201, $"User successfully created, Id:{user.Id}"); // What is this anonymous type new {id=item.id}...
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login (UserDto user)
        {
            var userObj = await repository.GetUserAsync(user);

            bool isUserLegit = SecureHasher.Verify(user.Password, userObj.Password);
            if (!isUserLegit)
            {
                return StatusCode(401, "Login Failed");
            }
            return StatusCode(200, "User login successful");
        }
        [HttpPost]
        [Route("Recover")]
        public async Task<ActionResult<User>> Recover (recoverUserDto emailDto)
        {
            Console.WriteLine(emailDto.Email);
            UserDto user = new UserDto{
                Email=emailDto.Email,
                Password="TemporaryPassword"
            };
            Console.WriteLine(user);
            var userObj = await repository.GetUserAsync(user);
            if (userObj == null) 
            {
                return StatusCode(404, "User not found with that email");
            }
            return userObj;
        }
    }
}
