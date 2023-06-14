using Microsoft.AspNetCore.Mvc;
using PH.UberConnect.Api;
using PH.UberConnect.Api.EndpointsBody;
using PH.UberConnect.Api.Responses;
using PH.UberConnect.Core.Enums;
using PH.UberConnect.Core.Models;
using PH.UberConnect.Core.Service;
using PH.UberConnect.Web.AppSettings;
using PH.UberConnect.Web.Models;
using System.Diagnostics;

namespace PH.UberConnect.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly Apis _apis;
        private readonly Services _services;
        private readonly ApiSettings _apiSettings;

        public HomeController(Apis apis, Services services, ApiSettings apiSettings)
        {
            _apis = apis;
            _services = services;
            _apiSettings = apiSettings;
        }

        public IActionResult Index()
        {
            var storeList = _services.LocalesService.Get().OrderBy(store => store.Descripcion).ToList();
            var homeViewModel = new HomeViewModel(storeList);
            return View(homeViewModel);
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel model)
        {
            var listDeliveriesViewModel = BuildListDeliveriesViewModel(model);
            listDeliveriesViewModel.DeliveryFilter = DeliveryFilter.OnGoing;
            listDeliveriesViewModel.ApplyFilter();
            return View("ListDeliveries", listDeliveriesViewModel);
        }

        [HttpGet]
        public IActionResult ListDeliveries(string storeId, string filter)
        {
            var homeViewModel = new HomeViewModel { StoreIdSelected = storeId };

            try
            {
                var listDeliveriesViewModel = BuildListDeliveriesViewModel(homeViewModel);
                listDeliveriesViewModel.DeliveryFilter = Enum.Parse<DeliveryFilter>(filter);
                listDeliveriesViewModel.ApplyFilter();
                return View(listDeliveriesViewModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private ListDeliveriesViewModel BuildListDeliveriesViewModel(HomeViewModel homeViewModel)
        {
            var storeList = _services.LocalesService.Get();
            var facturaList = _services.DeliveryService.GetToday();
            var storeSelected = new Local();
            var deliveriesItems = new List<DeliveryItemViewModel>();

            if (!string.IsNullOrEmpty(homeViewModel.StoreIdInputValue))
            {
                storeSelected = storeList.FirstOrDefault(store => store.CodRest == homeViewModel.StoreIdInputValue);
            }

            if (storeSelected?.CodRest == null)
            {
                storeSelected = storeList.FirstOrDefault(store => store.CodRest == homeViewModel.StoreIdSelected);
            }

            var currentToken = HttpContext.Session.GetString("_Token");

            if (currentToken == null)
            {
                currentToken = GetNewToken();
                HttpContext.Session.SetString("_Token", currentToken);
            }

            var endpointsParams = new EndpointParams(currentToken);
            var apiResponse = _apis.ListDeliveries.Execute<ListDeliveriesResponse>(endpointsParams);

            if (apiResponse.data == null)
            {
                return new ListDeliveriesViewModel(new List<DeliveryItemViewModel>(), storeSelected!);
            }

            var deliveries = apiResponse!.data!
                                .Where(delivery => delivery.pickup!.external_store_id == storeSelected!.CodRest)
                                .Where(delivery => delivery.created.AddHours(-6) > DateTime.Today)
                                .OrderByDescending(delivery => delivery.created)
                                .ToList();

            foreach (var deliveryResponse in deliveries)
            {
                var numFacturaCall = "-";

                if (facturaList.Any(fac => fac.IdFactura.ToString() == deliveryResponse.manifest.reference))
                {
                    numFacturaCall = facturaList.First(fac => fac.IdFactura.ToString() == deliveryResponse.manifest.reference).NumFacturaCall;
                }

                var deliveryItem = new DeliveryItemViewModel(numFacturaCall, deliveryResponse);
                deliveriesItems.Add(deliveryItem);
            }

            var listDeliveriesViewModel = new ListDeliveriesViewModel(deliveriesItems, storeSelected!);
            return listDeliveriesViewModel;
        }

        [HttpPost]
        public IActionResult CancelDelivery(CancelDeliveryViewModel model)
        {
            var currentToken = HttpContext.Session.GetString("_Token");

            if (currentToken == null)
            {
                currentToken = GetNewToken();
                HttpContext.Session.SetString("_Token", currentToken);
            }

            var endpointsParams = new EndpointParams(currentToken);
            var optionalParms = new Dictionary<string, string>() { { "deliveryId", model.DeliveryId } };

            try
            {
                var response = _apis.CancelDelivery.Execute<DeliveryResponse>(endpointsParams, optionalParms);

                if (response != null)
                {
                    _services.DeliveryService.UpdateCancellationReason(model.ReasonCancellation, model.PersonInCharge, model.DeliveryId);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Requests the Uber API to get a new token for execute other APIs.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetNewToken()
        {
            var authEndpointBody = new AuthenticationBody(_apiSettings.ClientId,
                 _apiSettings.ClientSecret,
                 _apiSettings.GrantType,
                 _apiSettings.Scope);

            var authEndpointParams = new EndpointParams(body: authEndpointBody);
            var authResponse = _apis.Authentication.Execute<AuthenticationResponse>(authEndpointParams);

            if (authResponse.access_token == null)
            {
                throw new Exception("No se pudo obtener el token");
            }

            return authResponse.access_token;
        }

        [HttpPost]
        public IActionResult UpdatePreparationTime(string codRest, int minutosDT)
        {
            var rowsAffected = _services.LocalesService.UdpateMinutosDT(codRest, minutosDT);

            if (rowsAffected >= 1) return Ok();

            return BadRequest();
        }
    }
}