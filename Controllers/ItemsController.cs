using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Server.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [ApiController]
    [Route("items")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IInMenuItemsRepository repository;

        public ItemsController(IInMenuItemsRepository repo)
        {
            this.repository = repo;
        }
        // GET Items/
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync () 
        {
            var items = (await repository.GetItemsAsync()).Select(item => item.AsDto());
            return items;
        }

        // GET Items/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id) { // In ASP core .NET v3? -> the suffix "Async" is removed, to fix rhis add controller to startup.cs
            var item = await repository.GetItemAsync(id);
            if (item is null) {
                return NotFound();
            }
            return Ok(item.AsDto());
        }

        // POST items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync (CreateItemDto itemDto) { // Conventional to return the created Dto on successful POST
            Item item = new() {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await repository.createItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id= item.Id }, item.AsDto()); // What is this anonymous type new {id=item.id}...
        }

        // PUT Items/id
        [HttpPut("{id}")]
        public async Task<ActionResult> updateItem(Guid id, updateItemDto itemDto) {
            var existingItem = await repository.GetItemAsync(id);

            if (existingItem is null) {
                return NotFound();
            }

            Item updatedItem = existingItem with {
                Name = itemDto.Name,
                Price= itemDto.Price, 
            };

            await repository.updateItemAsync(updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteItem(Guid id) {
            var existingItem = await repository.GetItemAsync(id);
            if (existingItem is null) {
                return NotFound();
            }
            await repository.deleteItemAsync(id);
            return NoContent();
        }
    }
}