using amount_in_words.Abstractions;
using amount_in_words.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace amount_in_words.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class AmountToWordController : ControllerBase
    {
        private readonly ILogger<AmountToWordController> _logger;
        private readonly IAmountToWordConverter _converter;

        public AmountToWordController(ILogger<AmountToWordController> logger, IAmountToWordConverter converter)
        {
            _logger = logger;
            _converter = converter;
        }

        [HttpPost]
        public IActionResult GetAmountInWord([FromBody] AmountRequest request)
        {
            return GetAmountInWord(request.Amount, request.ConvertCents);
        }

        private IActionResult GetAmountInWord(double amount, bool convertCents)
        {
            try
            {
                NumberFormatInfo f = new NumberFormatInfo { NumberGroupSeparator = " " };
                return Ok(new AmountResponse
                {
                    Amount = amount.ToString("#,0.00", f),
                    Word = _converter.CurrencyToWord(amount, convertCents)
                });;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ObjectResult(new WrongResponse { Error = ex.Message })
                {
                    StatusCode = 500
                };
            }

        }
    }
}
