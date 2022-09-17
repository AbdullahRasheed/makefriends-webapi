namespace makefriends_web_api.Database
{
    public class UserDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string LoginCollectionName { get; set; } = null!;
    }
}
