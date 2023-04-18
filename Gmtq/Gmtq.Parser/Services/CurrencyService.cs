using System.Collections;
using Gmtq.Data;
using Gmtq.Data.Models;
using Gmtq.Parser.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gmtq.Parser.Services;

public class CurrencyService : ICurrencyService
{
    private readonly CurrencyContext _currencyContext;
    private readonly ICurrencyApiService _apiService;
    private readonly ILogger<CurrencyService> _logger;

    public CurrencyService(
        CurrencyContext currencyContext,
        ICurrencyApiService apiService,
        ILogger<CurrencyService> logger)
    {
        _currencyContext = currencyContext;
        _apiService = apiService;
        _logger = logger;
    }
    
    public async Task LoadCurrencies(int year, CancellationToken token)
    {
        try
        {
            var currencies = await _apiService.GetCurrencies(year, token).ConfigureAwait(false);

            // TODO possible performance issues
            // consider raw sql, temp table, bulk libs, merge
            var existing = new HashSet<Currency>(new CurrencyEqualityComparer());
            
            await foreach (var element in _currencyContext.Currencies
                               .AsNoTracking()
                               .Select(x => new Currency { Date = x.Date, Name = x.Name})
                               .Where(x => x.Date.Year == year).AsAsyncEnumerable().WithCancellation(token))
            {
                existing.Add(element);
            }
                        
            foreach (var currency in currencies)
            {
                if (existing.Contains(currency))
                {
                    _currencyContext.Currencies.Update(currency);
                }
                else
                {
                    _currencyContext.Currencies.Add(currency);
                }
            }

            await _currencyContext.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during loading currencies");
            throw;
        }
    }
    
    private class CurrencyEqualityComparer : IEqualityComparer<Currency>
    {
        public bool Equals(Currency x, Currency y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Date.Equals(y.Date) && x.Name == y.Name;
        }

        public int GetHashCode(Currency obj)
        {
            return HashCode.Combine(obj.Date, obj.Name);
        }
    }
}