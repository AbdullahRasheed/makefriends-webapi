namespace makefriends_web_api.Data
{
    public class User
    {

        public string Username { get; private set; } = string.Empty;

        public byte[] PasswordHash { get; private set; } = new byte[0];

        public byte[] PasswordSalt { get; private set; } = new byte[0];

        public User(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
