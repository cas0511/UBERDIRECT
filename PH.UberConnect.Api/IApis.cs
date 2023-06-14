using PH.UberConnect.Api.Endpoints;

namespace PH.UberConnect.Api
{
    public interface IApis
    {
        Authentication Authentication { get; }
        CreateDelivery CreateDelivery { get; }
        GetDelivery GetDelivery { get; }
        ListDeliveries ListDeliveries { get; }
        CancelDelivery CancelDelivery { get; }
    }
}
