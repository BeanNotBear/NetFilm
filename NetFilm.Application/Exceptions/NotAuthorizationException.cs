namespace NetFilm.Application.Exceptions
{
    public class NotAuthorizationException : Exception
    {
        public NotAuthorizationException(string? message) : base(message)
        {
        }
    }
}
