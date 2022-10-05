using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class DataNotFoundException: Exception
    {
        public DataNotFoundException()
        {

        }

        public DataNotFoundException(string message): base(message)
        {

        }

        public DataNotFoundException(string message, Exception innerException): base(message, innerException)
        {

        }

        protected DataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
