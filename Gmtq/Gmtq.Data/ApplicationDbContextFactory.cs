using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gmtq.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<CurrencyContext>
{
    
    public CurrencyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CurrencyContext>();
        // TODO read properly from configs
        const string connectionString = "data source=localhost, 1433;initial catalog=CurrencyDb;user id=sa;password=VeryStr0ng@Pass";
        optionsBuilder.UseSqlServer(connectionString);

        return new CurrencyContext(optionsBuilder.Options);   
    }
}