using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Server.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Utilities;

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
        public async Task<paginatedResponse<IEnumerable<ItemDto>>> GetItemsAsync ([FromQuery] int skip, [FromQuery] int take, [FromQuery] int price, [FromQuery] IEnumerable<string> categories, [FromQuery] string keyword) 
        {
            var items = (await repository.GetItemsAsync(price, categories, keyword)).Select(item => item.AsDto());
            var paginatedItems = items.Skip(skip).Take(take);
            var response = new paginatedResponse<IEnumerable<ItemDto>> {
                itemsCount = items.Count(),
                data = paginatedItems,
            };

            return response;
        }

        // GET Items/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync([FromQuery] Guid id) { // In ASP core .NET v3? -> the suffix "Async" is removed, to fix rhis add controller to startup.cs
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
                Category = itemDto.Category,
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