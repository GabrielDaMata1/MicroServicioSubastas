
using System;
namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al realizar cualquier tipo de operación en la bases de datos en MongoDB.
    /// </summary>
    public class MongoRepositoryException : System.Exception
        {
            public MongoRepositoryException() { }
            public MongoRepositoryException(string message, System.Exception innerException) : base(message, innerException) { }
        }
    }
