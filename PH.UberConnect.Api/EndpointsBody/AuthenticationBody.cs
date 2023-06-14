namespace PH.UberConnect.Api.EndpointsBody
{
    public class AuthenticationBody : IEndpointBody
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }

        public AuthenticationBody(string clientId, string clientSecret, string grantType, string pScope)
        {
            client_id = clientId;
            client_secret = clientSecret;
            grant_type = grantType;
            scope = pScope;
        }
    }
}
