using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Entities;
using System.Collections.Generic;

namespace Server.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly InMenuItemsRepository repository;

        public ItemsController()
        {
            repository = new InMenuItemsRepository();
        }
        [HttpGet]
        public IEnumerable<Item> GetItems () 
        {
            var items = repository.GetItems();
            return items;
        }
    }
}