namespace PH.UberConnect.Api.EndpointsBody
{
    public class EndpointParams
    {
        public string BearerToken { get; set; }
        public IEndpointBody? Body { get; set; }

        public EndpointParams(string bearerToken, IEndpointBody body)
        {
            BearerToken = bearerToken;
            Body = body;
        }

        public EndpointParams(IEndpointBody body)
        {
            Body = body;
        }

        public EndpointParams(string bearerToken)
        {
            BearerToken = bearerToken;
        }
    }
}
