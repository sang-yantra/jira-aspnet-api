using System.Runtime.Serialization;

namespace Common.Exceptions
{
    public class DuplicateDataException : Exception
    {
        public DuplicateDataException()
        {

        }
        public DuplicateDataException(string message) : base(message)
        {

        }
        public DuplicateDataException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public DuplicateDataException(string conflictMessage, Guid conflictId)
        {
            Data["Message"] = conflictMessage;
            Data["ConflictId"] = conflictId;
        }

        protected DuplicateDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

    }
}
