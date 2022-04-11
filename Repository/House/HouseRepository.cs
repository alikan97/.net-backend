using System;
using System.Collections.Generic;
using Server.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Server.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        private const string dbName= "server";
        private const string collectionName = "house";

        private readonly IMongoCollection<House> houseCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public HouseRepository(IConfiguration config) 
        {
            var client = new MongoClient(config.GetConnectionString("mongoDb"));
            var database = client.GetDatabase(dbName);
            houseCollection = database.GetCollection<House>(collectionName);
        }

        public async Task addOccupant(House house)
        {
            await houseCollection.InsertOneAsync(house);
        }

        public Task createAlarmTrigger(string alarmName)
        {
            throw new NotImplementedException();
        }

        public async Task CreateHouse(House newhouse)
        {
            await houseCollection.InsertOneAsync(newhouse);
        }

        public Task deleteAlarm(string alarmName)
        {
            throw new NotImplementedException();
        }

        public Task updateAlarm(Dictionary<string, string> alarmState)
        {
            throw new NotImplementedException();
        }

        public Task updateHumidity(decimal newHumidity)
        {
            throw new NotImplementedException();
        }

        public Task updateTemperature(decimal newTemp)
        {
            throw new NotImplementedException();
        }

        public Task updateWeatherDescription(string newDescription)
        {
            throw new NotImplementedException();
        }
    }
}