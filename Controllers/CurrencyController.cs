namespace CurrenciesWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;            
        }

        [HttpGet("currencies")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetAllCurrencies()
        {
            var currencies = await _currencyService.GetCurrenciesAsync();

            if (currencies != null)
            {
                return Ok(currencies);
            }

            return BadRequest("Not aveilable currencies yet.");
        }

        [HttpGet("currencies/{id}")]
        public async Task<ActionResult<Currency>> GetCurrency(string id)
        {
            var currency = await _currencyService.GetCurrencyByIDAsync(id);

            if (currency != null)
            {
                return Ok(currency);
            }

            return BadRequest("Not aveilable currencies yet.");
        }
    }
}
