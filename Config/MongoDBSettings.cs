namespace Server.Settings
{
    public class MongoDBSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        private const string Password = "Abcd1234";
        public string ConnectionString 
        {
            get
            {
                return $"mongodb://{User}:{Password}@{Host}:{Port}/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false";
            }
        }
    }
}