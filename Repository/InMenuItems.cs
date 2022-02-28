using System.Collections.Generic;
using Server.Entities;
using System.Linq;
using System;
namespace Server.Repositories
{
    public class InMenuItemsRepository
    {
        private readonly List<Item> items = new() //Traditionally new List<Item> but now redundant
        {
            new Item {
                Id = Guid.NewGuid(),
                Name = "Potion",
                Price = 9.0M,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Item {
                Id = Guid.NewGuid(),
                Name = "Sword",
                Price = 20.0M,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Item {
                Id = Guid.NewGuid(),
                Name = "Shield",
                Price = 12.0M,
                CreatedDate = DateTimeOffset.UtcNow
            },
        };

        public IEnumerable<Item> GetItems() {
            return items;
        }

        public Item GetItem (Guid id) {
            return items.Where(items => items.Id == id).SingleOrDefault();
        }
    }
}