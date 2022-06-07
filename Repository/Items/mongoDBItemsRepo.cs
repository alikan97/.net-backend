using System;
using System.Collections.Generic;
using Server.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Server.Dtos;
using System.Linq;
using MongoDB.Bson.Serialization.Serializers;

namespace Server.Repositories
{
    public class MongoDBItemsRepository : IInMenuItemsRepository
    {
        private const string dbName = "server";
        private const string collectionName = "items";

        private readonly IMongoCollection<Item> itemsCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public MongoDBItemsRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongoDb"));
            var database = client.GetDatabase(dbName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }
        public async Task createItemAsync(Item item)
        {
            await itemsCollection.InsertOneAsync(item);
        }

        public async Task deleteItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(GetItemsFilters filters, string keyword)
        {
            try {
                // var it = await itemsCollection.Find(x => x.Price < filters.price).ToListAsync(); Using LINQ
                var stringFilter = filterBuilder.Regex("Name", new BsonRegularExpression(keyword));
                var categoryFilter = filterBuilder.In(x => x.Category, filters.Categories);
                var priceFilter = filterBuilder.Lte(x => x.Price, Convert.ToDecimal(filters.price));

                var combinedFilter = filterBuilder.And(stringFilter, categoryFilter, priceFilter);

                return await itemsCollection.Find(combinedFilter).ToListAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<Item>> GetItemsAsync(string keyword)
        {
            var filter = filterBuilder.AnyEq("item.name", new BsonRegularExpression($".*{keyword}.*"));
            return await itemsCollection.Find(filter).ToListAsync();
        }

        public async Task updateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}