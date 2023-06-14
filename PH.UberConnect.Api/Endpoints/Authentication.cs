using PH.UberConnect.Api.EndpointsBody;
using RestSharp;
using System.Reflection;
using System.Text.Json;

namespace PH.UberConnect.Api.Endpoints
{
    public class Authentication : IEndpoint
    {
        public static string Name { get => "oauth/v2/token"; }

        public Method Method { get => Method.Post; }

        public string BaseUrl { get; set; }

        public Authentication(string url)
        {
            BaseUrl = url;
        }

        public T? Execute<T>(EndpointParams endpointParams, Dictionary<string, string>? optionalParams = null)
        {
            endpointParams.Body = (AuthenticationBody)endpointParams.Body;
            var client = new RestClient($"{BaseUrl}{Name}");
            var request = new RestRequest
            {
                Method = Method,
                AlwaysMultipartFormData = true
            };

            foreach (PropertyInfo propertyInfo in endpointParams.Body.GetType().GetProperties())
            {
                request.AddParameter(propertyInfo.Name, propertyInfo.GetValue(endpointParams.Body)!.ToString());
            }

            var response = client.Execute(request);

            if (response.Content == null) return default;

            return JsonSerializer.Deserialize<T>(response.Content);
        }
    }
}
