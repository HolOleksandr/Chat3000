using System.Runtime.Serialization;

namespace Chat.BLL.Exceptions
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
