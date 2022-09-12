using System.Security.Cryptography;
using System.Text;

namespace makefriends_web_api.Util
{
    public class SHA512StringFunction : IHashFunction<string>
    {
        public byte[] GetHash(string t, out byte[] salt)
        {
            using(var func = new HMACSHA512())
            {
                salt = func.Key;
                return func.ComputeHash(Encoding.UTF8.GetBytes(t));
            }
        }

        public bool Verify(string t, byte[] hash, byte[] salt)
        {
            using(var func = new HMACSHA512(salt))
            {
                return hash.SequenceEqual(func.ComputeHash(Encoding.UTF8.GetBytes(t)));
            }
        }
    }
}
