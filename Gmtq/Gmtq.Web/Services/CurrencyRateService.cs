using Gmtq.Data;
using Gmtq.Data.Models;
using Gmtq.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Gmtq.Web.Services;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly CurrencyContext _currencyContext;

    public CurrencyRateService(
        CurrencyContext currencyContext)
    {
        _currencyContext = currencyContext;
    }
    
    public async Task<Currency?> GetCurrencyRate(string currency, DateTime date, CancellationToken token)
    {
        var currencyRate = await _currencyContext.Currencies
            .AsNoTracking()
            .Where(c => c.Name == currency && c.Date <= date)
            .OrderByDescending(c => c.Date)
            .FirstOrDefaultAsync(token);
        
        return currencyRate;
    }

    public async Task<IReadOnlyCollection<string>> GetCurrencyNames(CancellationToken token)
    {
        // TODO not an efficient way
        var currencyNames = await _currencyContext.Currencies
            .AsNoTracking()
            .Select(x => x.Name)
            .Distinct()
            .ToArrayAsync(token);
        
        return currencyNames;
    }
}