using System.Collections.Generic;
using Server.Entities;
using System.Linq;
using System;
namespace Server.Repositories
{
    public class InMenuItemsRepository : IInMenuItemsRepository
    {
        private readonly List<Item> items = new() //Traditionally new List<Item> but now redundant
        {
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Potion",
                Price = 9.0M,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Sword",
                Price = 20.0M,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Shield",
                Price = 12.0M,
                CreatedDate = DateTimeOffset.UtcNow
            },
        };

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid id)
        {
            return items.Where(items => items.Id == id).SingleOrDefault();
        }

        public void createItem(Item item)
        {
            items.Add(item);
        }

        public void updateItem(Item item)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;
        }

        public void deleteItem(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
        }
    }
}