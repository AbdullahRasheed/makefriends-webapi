using makefriends_web_api.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace makefriends_web_api.Database
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IOptions<UserDatabaseSettings> userDatabaseSettings)
        {
            var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(userDatabaseSettings.Value.LoginCollectionName);
        }

        public async Task<User?> FindAsync(string id) => await _userCollection.Find(user => id.ToString() == user.Id.ToString()).FirstOrDefaultAsync();

        public async Task<User?> FindByUsernameAsync(string username) => await _userCollection.Find(user => username == user.Username).FirstOrDefaultAsync();

        public async Task InsertAsync(User user) => await _userCollection.InsertOneAsync(user);

        public async Task ReplaceAsync(string id, User user) => await _userCollection.ReplaceOneAsync(user => id.ToString() == user.Id.ToString(), user);

        public async Task DeleteAsync(string id) => await _userCollection.DeleteOneAsync(user => id.ToString() == user.Id.ToString());
    }
}
