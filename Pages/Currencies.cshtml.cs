namespace CurrenciesWebService.Pages
{
    public class CurrenciesModel : PageModel
    {
        private HttpClient _httpClient = new HttpClient();

        [BindProperty]
        public List<Currency> CurrenciesPagination { get; set; } = new List<Currency>();

        public List<Currency> Currencies { get; set; } = new List<Currency>();

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty]
        public Currency Currency { get; set; } = new Currency();

        public int Count { get; set; }

        public int PageSize { get; set; } = 8;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public bool ShowPrevious => CurrentPage > 1;

        public bool ShowNext => CurrentPage < TotalPages;

        public async Task OnGetAsync()
        {
            var request = await _httpClient.GetFromJsonAsync<IEnumerable<Currency>>("https://localhost:7240/api/Currency/currencies");

            if (request != null)
            {
                Currencies = request.ToList();
            }

            Count = Currencies.Count;

            CurrenciesPagination = GetPaginatedResult(CurrentPage, PageSize);
        }

        private List<Currency> GetPaginatedResult(int currentPage, int pageSize)
        {    
            return Currencies.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }     
        
        public async Task OnPostByCodeAsync(string code)
        {
            var currency = await _httpClient.GetFromJsonAsync<Currency>($"https://localhost:7240/api/Currency/currencies/{code}");

            if(currency != null)
            {
                Currency = currency;
            }
        }

        public async Task<PartialViewResult> OnGetProductAsync(string code)
        {
            return Partial("_ProductDetails", await _httpClient.GetFromJsonAsync<Currency>($"https://localhost:7240/api/Currency/currencies/{code}"));
        }

        public async Task<JsonResult> OnGetProductAsJson(string code)
        {
            return new JsonResult( await _httpClient.GetFromJsonAsync<Currency>($"https://localhost:7240/api/Currency/currencies/{code}"));
        }
    }
}
