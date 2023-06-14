using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.UberConnect.Api.Responses.Delivery
{
    public class VerificationRequirements
    {
        public List<Barcode> barcodes { get; set; }
        public bool picture { get; set; }
    }
}
