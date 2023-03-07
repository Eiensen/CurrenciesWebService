namespace CurrenciesWebService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<Currency> Currencies { get; set; }        
    }
}
