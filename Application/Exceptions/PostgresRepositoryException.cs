namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al realizar cualquier tipo de operación en la bases de datos en PostgreSQL.
    /// </summary>
    public class PostgresRepositoryException: System.Exception
    {
        public PostgresRepositoryException() { }
        public PostgresRepositoryException(string message, System.Exception innerException) : base("message", innerException) { }
    }
}
