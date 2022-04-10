namespace Server.Settings
{
    public class IMongoDBSettings
    {
        string Host { get; set; }
        int Port { get; set; }
        string User { get; set; }
        string Password { get; set; }
        string Database { get; set; }
        string ConnectionString {get;}
    }
}