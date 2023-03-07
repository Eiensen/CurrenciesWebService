namespace CurrenciesWebService.Services
{
    public class CurrencyService : ICurrencyService
    {      
        private const string CURR_URI = "https://www.cbr-xml-daily.ru/daily_json.js";

        private HttpClient _httpClient = new HttpClient();

        private readonly AppDbContext _context;

        private JsonElement _jsonElement = new JsonElement();

        private Currency? _currency = new Currency();

        public CurrencyService(AppDbContext context)
        {
            _context = context;

            _jsonElement = GetCurrenciesFromJsonAsync().Result;
        }

        private async Task<string> GetJsonStringFromUriAsync()
        {
            var response = await _httpClient.GetAsync(CURR_URI);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                return result;
            }

            return string.Empty;
        }

        private async Task<JsonElement> GetCurrenciesFromJsonAsync()
        {
            var response = await GetJsonStringFromUriAsync();

            if (!response.IsNullOrEmpty())
            {
                var responseJSON = JsonDocument.Parse(response);

                var root = responseJSON.RootElement;

                var jsonValues = root.GetProperty("Valute");

                return jsonValues;
            }
            return new JsonElement();
        }

        private async Task UpdateCurrenciesInDatabaseAsync()
        {
            foreach (var item in _jsonElement.EnumerateObject())
            {
                var jsonElement = item.Value;

                _currency = JsonSerializer.Deserialize<Currency>(jsonElement);

                if (_currency != null)
                {
                    var currencyInDbToUpdate = await _context.Currencies.FirstOrDefaultAsync(c => c.ID == _currency.ID && c.Value != _currency.Value);

                    if (currencyInDbToUpdate != null)
                    {
                        currencyInDbToUpdate.ID = _currency.ID;
                        currencyInDbToUpdate.Name = _currency.Name;
                        currencyInDbToUpdate.CharCode = _currency.CharCode;
                        currencyInDbToUpdate.NumCode = _currency.NumCode;
                        currencyInDbToUpdate.Previous = _currency.Previous;
                        currencyInDbToUpdate.Value = _currency.Value;
                        currencyInDbToUpdate.Nominal = _currency.Nominal;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            await UpdateCurrenciesInDatabaseAsync();

            return _context.Currencies;
        }

        public async Task<Currency?> GetCurrencyByIDAsync(string id)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.ID == id);

            if(currency != null)
            {
                return currency;
            }
            else 
            {
                return null;
            }            
        }
    }
}
