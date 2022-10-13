using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace makefriends_web_api.Database
{
    public class AvatarService
    {

        private readonly GridFSBucket _bucket;

        public AvatarService(IOptions<UploadDatabaseSettings> avatarDatabaseSettings)
        {
            var mongoClient = new MongoClient(avatarDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(avatarDatabaseSettings.Value.DatabaseName);

            _bucket = new GridFSBucket(mongoDatabase, new GridFSBucketOptions
            {
                BucketName = avatarDatabaseSettings.Value.AvatarBucketName,
                ChunkSizeBytes = 16777216, // 16 MBs
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            });
        }

        public async Task<ObjectId> InsertAvatar(IFormFile file, string name) => await _bucket.UploadFromStreamAsync(name, file.OpenReadStream());

        public async Task<byte[]> FindAvatar(ObjectId id) => await _bucket.DownloadAsBytesAsync(id);

        public async Task<byte[]> FindAvatarByName(string name) => await _bucket.DownloadAsBytesByNameAsync(name);
    }
}
