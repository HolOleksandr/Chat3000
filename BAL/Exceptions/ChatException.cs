using System.Runtime.Serialization;

namespace BAL.Exceptions
{
    [Serializable]
    public class ChatException : Exception
    {
        public ChatException() { }

        public ChatException(string message) : base(message) { }

        public ChatException(string message, Exception innerException) : base(message, innerException) { }

        protected ChatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
