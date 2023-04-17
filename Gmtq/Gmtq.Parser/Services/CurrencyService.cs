using Gmtq.Data;
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

            foreach (var currency in currencies)
            {
                _currencyContext.Currencies.Update(currency);
            }

            await _currencyContext.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during loading currencies");
            throw;
        }
        
    }
}