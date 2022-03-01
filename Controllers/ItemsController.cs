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
    }
}