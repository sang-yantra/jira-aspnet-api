using System.Runtime.Serialization;

namespace Common.Exceptions
{
    public class DatabaseUpdateFailureException : Exception
    {
        public DatabaseUpdateFailureException()
        {

        }

        public DatabaseUpdateFailureException(string message) : base(message)
        {

        }

        public DatabaseUpdateFailureException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected DatabaseUpdateFailureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
