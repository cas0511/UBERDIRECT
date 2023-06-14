using PH.UberConnect.Api.Endpoints;

namespace PH.UberConnect.Api
{
    public class Apis : IApis
    {
        private string _url;

        private string _authUrl;

        private string _customerId;

        private Authentication? _authentication;

        private CreateDelivery? _createDelivery;

        private GetDelivery? _getDelivery;

        private ListDeliveries? _listDeliveries;

        private CancelDelivery _cancelDelivery;

        public Authentication Authentication { get { return _authentication ??= new Authentication(_authUrl); } }

        public CreateDelivery CreateDelivery { get { return _createDelivery ??= new CreateDelivery(_url, _customerId); } }

        public GetDelivery GetDelivery { get { return _getDelivery ??= new GetDelivery(_url, _customerId); } }

        public ListDeliveries ListDeliveries { get { return _listDeliveries ??= new ListDeliveries(_url, _customerId); } }

        public CancelDelivery CancelDelivery { get { return _cancelDelivery ??= new CancelDelivery(_customerId, _url); } }

        public Apis(string url, string authUrl, string customerId)
        {
            _url = url;
            _authUrl = authUrl;
            _customerId = customerId;
        }
    }
}
