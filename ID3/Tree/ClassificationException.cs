using System;
using System.Runtime.Serialization;

namespace C45.Tree
{
    [Serializable]
    internal class ClassificationException : Exception
    {
        public ClassificationException()
        {
        }

        public ClassificationException(string message) : base(message)
        {
        }

        public ClassificationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClassificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}