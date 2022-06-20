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
using Microsoft.IdentityModel.Tokens;

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

        public async Task<IEnumerable<Item>> GetItemsAsync(int price, IEnumerable<string> categories, string keyword)
        {
            try {
                string[] stringCast = categories.Select(x => x).ToArray();
                FilterDefinition<Item> combinedFilters = filterBuilder.Empty;
                // var it = await itemsCollection.Find(x => x.Price < filters.price).ToListAsync(); Using LINQ

                if (String.IsNullOrEmpty(keyword) && categories.IsNullOrEmpty() && price <= 0) {
                    return await itemsCollection.Find(new BsonDocument()).ToListAsync();
                }

                if (!String.IsNullOrEmpty(keyword)) {
                    combinedFilters &= filterBuilder.Regex("Name", new BsonRegularExpression(keyword));
                }

                if (!categories.IsNullOrEmpty()) {
                    combinedFilters &= filterBuilder.In(x => x.Category, stringCast);
                }

                if (price > 0) {
                    combinedFilters &= filterBuilder.Lte(x => x.Price, Convert.ToDecimal(price));
                }

                return await itemsCollection.Find(combinedFilters).ToListAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task updateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}