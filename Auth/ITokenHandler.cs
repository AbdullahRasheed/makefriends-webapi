namespace makefriends_web_api.Auth
{
    public interface ITokenHandler<T>
    {

        public string GenerateToken(T t);
    }
}
