using PH.UberConnect.Api.EndpointsBody;
using PH.UberConnect.Api.Responses;
using PH.UberConnect.Api.Utils;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace PH.UberConnect.Api.Endpoints
{
    public class ListDeliveries : IPrivateEndpoint
    {
        public string CustomerId { get; set; }
        public string BaseUrl { get; set; }

        public ListDeliveries(string url, string customerId)
        {
            CustomerId = customerId;
            BaseUrl = url;
        }

        public Method Method => Method.Get;

        public T? Execute<T>(EndpointParams endpointParams, Dictionary<string, string>? optionalParams = null)
        {
            var client = new RestClient($"{BaseUrl}v1/customers/{CustomerId}/deliveries");
            var request = new RestRequest
            {
                Method = Method
            };
            request.AddHeader("Authorization", $"Bearer {endpointParams.BearerToken}");
            var response = client.Execute(request);

            if (response.Content == null) throw new Exception("No hay respuesta del servicio de Uber");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
                var spanishMessage = ExceptionUtil.GetMessageInSpanish(error!);
                throw new Exception(spanishMessage);
            }

            return JsonSerializer.Deserialize<T>(response.Content);
        }
    }
}
