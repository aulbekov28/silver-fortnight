using Gmtq.Data.Models;

namespace Gmtq.Parser.Services.Abstractions;

public interface ICurrencyApiService
{
    Task<IReadOnlyCollection<Currency>> GetCurrencies(int year, CancellationToken token);
}