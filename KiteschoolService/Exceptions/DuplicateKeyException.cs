
namespace KiteschoolService.Exceptions
{
    public class DuplicateKeyException : Exception
    {
        public DuplicateKeyException(string message) : base(message)
        {
        }

        public DuplicateKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class KiteschoolCreationException : Exception
    {
        public KiteschoolCreationException(string message) : base(message)
        {
        }

        public KiteschoolCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
