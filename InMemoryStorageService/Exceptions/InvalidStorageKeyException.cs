using System;

namespace InMemoryStorageService.Exceptions
{
    public class NotExistsStorageKeyException: Exception
    {
        public NotExistsStorageKeyException()
        { }

        public NotExistsStorageKeyException(string message)
            : base(message)
        { }

        public NotExistsStorageKeyException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
