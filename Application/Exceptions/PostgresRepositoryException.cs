namespace Application.Exceptions
{
    public class PostgresRepositoryException: System.Exception
    {
        public PostgresRepositoryException() { }
        public PostgresRepositoryException(string message, System.Exception innerException) : base("message", innerException) { }
    }
}
