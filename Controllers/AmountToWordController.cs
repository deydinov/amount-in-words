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

        [HttpGet]
        public IActionResult GetAmountInWord([FromQuery] decimal amount, [FromQuery] bool convertCents = true)
        {
            var resp = new AmountResponse
            {
                Result = _converter.CurrencyToWord(amount, convertCents)
            };

            return Ok(resp);
        }
    }
}
