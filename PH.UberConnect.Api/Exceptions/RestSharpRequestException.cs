namespace PH.UberConnect.Core.Exceptions
{

    [Serializable]
    public class RestSharpRequestException : Exception
    {
        public RestSharpRequestException() { }
        public RestSharpRequestException(string message) : base(message) { }
        public RestSharpRequestException(string message, Exception inner) : base(message, inner) { }
        protected RestSharpRequestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
