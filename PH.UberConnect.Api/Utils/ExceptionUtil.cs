using PH.UberConnect.Api.Responses;

namespace PH.UberConnect.Api.Utils
{
    public static class ExceptionUtil
    {
        public static string GetMessageInSpanish(ErrorResponse errorResponse)
        {
            return errorResponse.code switch
            {
                "noncancelable_delivery" => "La entrega no puede ser cancelada.",
                "customer_not_cound" => "Cliente no existe.",
                "delivery_not_found" => "La entrega no fue encontrada",
                "request_timeout" => "La petición al servicio de Uber caducó. Intentelo de nuevo.",
                "unknown_error" => "Ocurrió un error inesperado en el servicio de Uber.",
                "service_unavailable" => "El servicio de Uber no esta disponible por el momento.",
                "unauthorized" => "No hay autorización para usar el servicio de Uber, se necesita renovar el token.",
                _ => errorResponse.message!,
            };
        }
    }
}
