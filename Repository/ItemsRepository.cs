using System;
using System.Collections.Generic;
using Server.Entities;

namespace Server.Repositories
{
    public interface IInMenuItemsRepository
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();
    }
}