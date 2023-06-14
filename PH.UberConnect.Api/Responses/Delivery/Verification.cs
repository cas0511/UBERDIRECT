using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.UberConnect.Api.Responses.Delivery
{
    public class Verification
    {
        public List<Barcode> barcodes { get; set; }
        public Picture picture { get; set; }
    }
}
