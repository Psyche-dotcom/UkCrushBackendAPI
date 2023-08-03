using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO.paypal
{
    public sealed class PaypalFee
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }
}
