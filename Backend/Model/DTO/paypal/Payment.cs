using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model.DTO.paypal
{
    public sealed class Payment
    {
        public List<Captures> captures { get; set; }
    }
}
