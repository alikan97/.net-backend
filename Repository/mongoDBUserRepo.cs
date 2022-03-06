using System;
using System.Threading.Tasks;
using Server.Repositories;
using MongoDB.Driver;
using MongoDB.Bson;
using Server.Entities;

namespace Server.Repositories
{
    public class MongoDBUserRepository : IUserRepository
    {
        private const string dbName = "server";
        private const string collectionName = "users";
        private readonly IMongoCollection<User> userCollection;
        
        public MongoDBUserRepository(IMongoClient mongoClient)
        {
            IMongoDatabase db = mongoClient.GetDatabase(dbName);
            userCollection = db.GetCollection<User>(collectionName);
        }
        public Task Login(string email, string Password)
        {
            throw new NotImplementedException();
        }

        public Task RecoverAccount(string email)
        {
            throw new NotImplementedException();
        }

        public async Task Register(User user)
        {
            await userCollection.InsertOneAsync(user);
        }
    }
}