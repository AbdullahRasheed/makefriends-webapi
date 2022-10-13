namespace makefriends_web_api.Database
{
    public class UploadDatabaseSettings
    {

        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string AvatarBucketName { get; set; } = null!;
    }
}
