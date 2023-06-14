using PH.UberConnect.Api.EndpointsBody;
using PH.UberConnect.Core.Exceptions;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace PH.UberConnect.Api.Endpoints
{
    public class CreateDelivery : IPrivateEndpoint
    {
        public CreateDelivery(string url, string customerId)
        {
            BaseUrl = url;
            CustomerId = customerId;
        }

        public string CustomerId { get; set; }
        public string BaseUrl { get; set; }
        public Method Method { get => Method.Post; }

        public T? Execute<T>(EndpointParams endpointParams, Dictionary<string, string>? optionalParams = null)
        {
            var client = new RestClient($"{BaseUrl}v1/customers/{CustomerId}/deliveries");
            var request = new RestRequest
            {
                Method = Method
            };
            request.AddHeader("Authorization", $"Bearer {endpointParams.BearerToken}");
            request.AddHeader("Content-Type", "application/json");
            var jsonBody = endpointParams.Body.ToString();
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var response = client.Execute(request);
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var exception = new RestSharpRequestException("Error al conectar con Uber connect API");
                exception.Data.Add("Status Code", response.StatusCode.ToString());
                exception.Data.Add("Response", response.Content);
                throw exception;
            }

            return JsonSerializer.Deserialize<T>(response.Content!);
        }
    }
}
