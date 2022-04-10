namespace Server.Settings
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string ConnectionString
        {
            get
            {
                //mongodb://Ali:Abcd1234@localhost:27017/?authSource=admin&readPreference=primary&directConnection=true&ssl=false
                return $"mongodb://{User}:{Password}@{Host}:{Port}";
            }
        }
    }
}