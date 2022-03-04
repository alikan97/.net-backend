using System;
using System.Collections.Generic;
using Server.Entities;
using MongoDB.Driver;

namespace Server.Repositories
{
    public class MongoDBItemsRepository : IInMenuItemsRepository
    {
        private const string dbName= "server";
        private const string collectionName = "items";

        private readonly IMongoCollection<Item> itemsCollection;

        public MongoDBItemsRepository(IMongoClient mongoClient) 
        {
            IMongoDatabase db = mongoClient.GetDatabase(dbName);
            itemsCollection = db.GetCollection<Item>(collectionName);
        }
        public void createItem(Item item)
        {
            itemsCollection.InsertOneAsync(item);
        }

        public void deleteItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Item GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItems()
        {
            throw new NotImplementedException();
        }

        public void updateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}