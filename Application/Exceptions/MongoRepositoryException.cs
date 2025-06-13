
using System;
namespace Application.Exceptions
{
        public class MongoRepositoryException : System.Exception
        {
            public MongoRepositoryException() { }
            public MongoRepositoryException(string message, System.Exception innerException) : base("message", innerException) { }
        }
    }
