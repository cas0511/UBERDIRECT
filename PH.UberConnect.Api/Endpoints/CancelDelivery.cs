using PH.UberConnect.Api.EndpointsBody;
using PH.UberConnect.Api.Responses;
using PH.UberConnect.Api.Utils;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace PH.UberConnect.Api.Endpoints
{
    public class CancelDelivery : IPrivateEndpoint
    {
        public string CustomerId { get; set; }
        public string BaseUrl { get; set; }
        public Method Method { get => Method.Post; }

        public CancelDelivery(string customerId, string baseUrl)
        {
            CustomerId = customerId;
            BaseUrl = baseUrl;
        }

        public T? Execute<T>(EndpointParams endpointParams, Dictionary<string, string>? optionalParams = null)
        {
            var client = new RestClient($"{BaseUrl}v1/customers/{CustomerId}/deliveries/{optionalParams["deliveryId"]}/cancel");
            var request = new RestRequest
            {
                Method = Method
            };
            request.AddHeader("Authorization", $"Bearer {endpointParams.BearerToken}");
            var response = client.Execute(request);

            if (response.Content == null) return default;
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
                var spanishMessage = ExceptionUtil.GetMessageInSpanish(error);
                throw new Exception(spanishMessage);
            }

            return JsonSerializer.Deserialize<T>(response.Content);
        }
    }
}
