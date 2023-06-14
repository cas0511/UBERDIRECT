namespace PH.UberConnect.Api.Endpoints
{
    public interface IPrivateEndpoint : IEndpoint
    {
        public string CustomerId { get; set; }
    }
}
