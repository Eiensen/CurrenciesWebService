namespace CurrenciesWebService.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync();

        Task<Currency?> GetCurrencyByIDAsync(string id);
    }
}