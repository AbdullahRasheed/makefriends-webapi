using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace makefriends_web_api.Data
{
    public class User
    {

        [BsonId]
        public ObjectId Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[0];

        public byte[] PasswordSalt { get; set; } = new byte[0];

        public User(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            Id = ObjectId.GenerateNewId();
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
