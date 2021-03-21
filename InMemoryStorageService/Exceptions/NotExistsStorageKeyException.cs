using System;

namespace InMemoryStorageService.Exceptions
{
    public class InvalidStorageKeyException: Exception
    {
        public InvalidStorageKeyException()
        { }

        public InvalidStorageKeyException(string message)
            : base(message)
        { }

        public InvalidStorageKeyException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
