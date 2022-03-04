// using System.Collections.Generic;
// using Server.Entities;
// using System.Linq;
// using System;
// using System.Threading.Tasks;

// namespace Server.Repositories
// {
//     public class InMenuItemsRepository : IInMenuItemsRepository
//     {
//         private readonly List<Item> items = new() //Traditionally new List<Item> but now redundant
//         {
//             new Item
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Potion",
//                 Price = 9.0M,
//                 CreatedDate = DateTimeOffset.UtcNow
//             },
//             new Item
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Sword",
//                 Price = 20.0M,
//                 CreatedDate = DateTimeOffset.UtcNow
//             },
//             new Item
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Shield",
//                 Price = 12.0M,
//                 CreatedDate = DateTimeOffset.UtcNow
//             },
//         };

//         public IEnumerable<Item> GetItemsAsync()
//         {
//             return items;
//         }

//         public Item GetItemAsync(Guid id)
//         {
//             return items.Where(items => items.Id == id).SingleOrDefault();
//         }

//         public Task createItemAsync(Item item)
//         {
//             items.Add(item);
//         }

//         public void updateItemAsync(Item item)
//         {
//             var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
//             items[index] = item;
//         }

//         public void deleteItemAsync(Guid id)
//         {
//             var index = items.FindIndex(existingItem => existingItem.Id == id);
//             items.RemoveAt(index);
//         }
//     }
// }