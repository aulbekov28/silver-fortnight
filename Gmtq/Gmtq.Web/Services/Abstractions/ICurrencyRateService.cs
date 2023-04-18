using Gmtq.Data.Models;

namespace Gmtq.Web.Services.Abstractions;

public interface ICurrencyRateService
{
    Task<Currency?> GetCurrencyRate(string currency, DateTime date, CancellationToken token);
    
    Task<IReadOnlyCollection<string>> GetCurrencyNames(CancellationToken token);
}