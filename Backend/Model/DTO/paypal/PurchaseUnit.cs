using Model.Enitities;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO.paypal
{
    public sealed class PurchaseUnit
    {
        public Amounts amount { get; set; }
        public string reference_id { get; set; }
        public Shipping shipping { get; set; }
        public Payment payments { get; set; }
    }

}
