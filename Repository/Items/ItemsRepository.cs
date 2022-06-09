using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Dtos;
using Server.Entities;

namespace Server.Repositories
{
    public interface IInMenuItemsRepository
    {
        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync(int price, IEnumerable<string> categories, string keyword);
        Task createItemAsync(Item item);
        Task updateItemAsync(Item item);
        Task deleteItemAsync(Guid id);
    }
}