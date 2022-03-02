using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Server.Dtos;

namespace Server.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IInMenuItemsRepository repository;

        public ItemsController(IInMenuItemsRepository repo)
        {
            this.repository = repo;
        }
        // GET Items/
        [HttpGet]
        public IEnumerable<ItemDto> GetItems () 
        {
            var items = repository.GetItems().Select( item => item.AsDto());
            return items;
        }

        // GET Items/id
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id) {
            var item = repository.GetItem(id);
            if (item is null) {
                return NotFound();
            }
            return Ok(item.AsDto());
        }

        // POST items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem (CreateItemDto itemDto) { // Conventional to return the created Dto on successful POST
            Item item = new() {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            repository.createItem(item);
            return CreatedAtAction(nameof(GetItem), new { id= item.Id }, item.AsDto()); // What is this anonymous type new {id=item.id}...
        }

        // PUT Items/id
        [HttpPut("{id}")]
        public ActionResult updateItem(Guid id, updateItemDto itemDto) {
            var existingItem = repository.GetItem(id);

            if (existingItem is null) {
                return NotFound();
            }

            Item updatedItem = existingItem with {
                Name = itemDto.Name,
                Price= itemDto.Price, 
            };

            repository.updateItem(updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult deleteItem(Guid id) {
            var existingItem = repository.GetItem(id);
            if (existingItem is null) {
                return NotFound();
            }
            repository.deleteItem(id);
            return NoContent();
        }
    }
}