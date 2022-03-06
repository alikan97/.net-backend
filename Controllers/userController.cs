using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Server.Dtos;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("users")]
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
            User user = new() {
                Id = Guid.NewGuid(),
                Email = userDto.Email,
                Password = userDto.Password,
            };

            await repository.Register(user);
            return CreatedAtAction("User Registered", new { id= user.Id }, user.AsDto()); // What is this anonymous type new {id=item.id}...
        }
    }
}