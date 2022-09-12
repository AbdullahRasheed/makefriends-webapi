namespace makefriends_web_api.Util
{
    public interface IHashFunction<T>
    {

        public byte[] GetHash(T t, out byte[] salt);

        public bool Verify(T t, byte[] hash, byte[] salt);
    }
}
