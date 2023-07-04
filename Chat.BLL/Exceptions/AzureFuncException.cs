using System.Runtime.Serialization;

namespace Chat.BLL.Exceptions
{
    [Serializable]
    public class AzureFuncException : Exception
    {
        public AzureFuncException() { }

        public AzureFuncException(string message) : base(message) { }

        public AzureFuncException(string message, Exception innerException) : base(message, innerException) { }

        protected AzureFuncException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
