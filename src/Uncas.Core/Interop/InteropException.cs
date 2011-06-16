using System;
using System.Runtime.Serialization;

namespace Uncas.Core.Interop
{
    [Serializable]
    public class InteropException : Exception
    {
        public InteropException()
        {
        }

        public InteropException(string message)
            : base(message)
        {
        }

        public InteropException(string message, int errorCode)
            : this(message)
        {
            ErrorCode = errorCode;
        }

        public InteropException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InteropException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public int ErrorCode { get; private set; }

        public override void GetObjectData(
            SerializationInfo info,
            StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}