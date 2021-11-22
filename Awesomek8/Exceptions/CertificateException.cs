using System;
using System.Runtime.Serialization;

namespace Awesomek8.Exceptions
{
    [Serializable]
    public class CertificateException : Exception
    {
        public CertificateException() { }

        public CertificateException(string message) : base(message) { }

        public CertificateException(string message, Exception innerException) : base(message, innerException) { }

        protected CertificateException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}
