using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace amount_in_words.Abstractions
{
    public interface IAmountToWordConverter
    {
        string CurrencyToWord(double amount, bool convertCents = true);
    }
}
