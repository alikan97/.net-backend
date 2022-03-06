using System;
using System.Threading.Tasks;
using Server.Repositories;
using MongoDB.Driver;
using Server.Dtos;
using Server.Entities;

namespace Server.Repositories
{
    public class MongoDBUserRepository : IUserRepository
    {
        private const string dbName = "server";
        private const string collectionName = "users";
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;
        private readonly IMongoCollection<User> userCollection;
        
        public MongoDBUserRepository(IMongoClient mongoClient)
        {
            IMongoDatabase db = mongoClient.GetDatabase(dbName);
            userCollection = db.GetCollection<User>(collectionName);
        }
        public async Task<User> GetUserAsync(UserDto user)
        {
            var filter = filterBuilder.Eq(item => item.Email, user.Email);
            return await userCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task Register(User user)
        {
            await userCollection.InsertOneAsync(user);
        }
    }
}