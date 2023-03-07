namespace CurrenciesWebService.Models
{
    public class Currency
    {
        public string ID { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string CharCode { get; set; } = string.Empty;

        public string NumCode { get; set; } = string.Empty;

        public int Nominal { get; set; }

        public double Value { get; set; }

        public double Previous { get; set; }
    }
}
