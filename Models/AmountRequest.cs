using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace amount_in_words.Models
{
    public class AmountRequest
    {
        public double Amount { get; set; }
        public bool ConvertCents { get; set; }
    }
}
