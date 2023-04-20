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

    private static readonly Func<CurrencyContext, string, DateTime, Task<Currency?>> GetCurrencyRateAsync =
        EF.CompileAsyncQuery((CurrencyContext context, string currency, DateTime date) =>
            context.Currencies
                .AsNoTracking()
                .Where(c => c.Name == currency && c.Date <= date)
                .OrderByDescending(c => c.Date)
                .FirstOrDefault());

    public async Task<Currency?> GetCurrencyRate(string currency, DateTime date, CancellationToken token)
    {
        var currencyRate = await GetCurrencyRateAsync(_currencyContext, currency, date);
        
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