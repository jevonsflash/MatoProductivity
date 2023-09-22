using Abp;
using System;
using System.Runtime.Serialization;

namespace MatoProductivity.Core.Location
{
    public class LocationResolveException : AbpException
    {
        public LocationResolveException()
        {
        }

        public LocationResolveException(string message)
            : base(message)
        {
        }

        public LocationResolveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public LocationResolveException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
