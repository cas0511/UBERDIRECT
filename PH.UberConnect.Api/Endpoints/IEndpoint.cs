using PH.UberConnect.Api.EndpointsBody;
using RestSharp;

namespace PH.UberConnect.Api.Endpoints
{
    public interface IEndpoint
    {
        public string BaseUrl { get; set; }

        public Method Method { get; }

        public T? Execute<T>(EndpointParams endpointParams, Dictionary<string, string>? optionalParams = null);
    }
}
