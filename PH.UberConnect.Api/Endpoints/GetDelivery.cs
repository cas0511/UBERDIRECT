using PH.UberConnect.Api.EndpointsBody;
using RestSharp;
using System.Text.Json;

namespace PH.UberConnect.Api.Endpoints
{
    public class GetDelivery : IPrivateEndpoint
    {
        public string CustomerId { get; set; }
        public string BaseUrl { get; set; }
        public Method Method { get => Method.Get; }

        public GetDelivery(string url, string customerId)
        {
            BaseUrl = url;
            CustomerId = customerId;
        }

        public T? Execute<T>(EndpointParams endpointParams, Dictionary<string, string>? optionalParams = null)
        {
            if (optionalParams == null) return default;

            endpointParams.Body = (GetDeliveryBody)endpointParams.Body;
            var deliveryId = endpointParams.GetType().GetProperties().FirstOrDefault(prop => prop.Name == "DeliveryId")?.GetValue(endpointParams);
            var client = new RestClient($"{BaseUrl}v1/customers/{CustomerId}/deliveries/{deliveryId}");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Authorization", $"Bearer {optionalParams["token"]}");
            var response = client.Execute(request);

            if (response.Content == null) return default;

            return JsonSerializer.Deserialize<T>(response.Content);
        }
    }
}
