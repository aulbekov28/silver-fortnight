using System.Globalization;
using System.Net;
using Gmtq.Data.Models;
using Gmtq.Parser.Models;
using Gmtq.Parser.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Gmtq.Parser.Services;

public class CurrencyApiService : ICurrencyApiService
{
    private readonly CurrencyApiConfig _config;

    public CurrencyApiService(IOptions<CurrencyApiConfig> options)
    {
        _config = options.Value;
    }

    public async Task<IReadOnlyCollection<Currency>> GetCurrencies(int year, CancellationToken token)
    {
        var url = _config.ApiUrl + year;

        using var httpClient = new HttpClient();
        var data = await httpClient.GetStringAsync(url, token);
        
        var lines = data.Split('\n');
        var currencies = new List<Currency>();

        for (var i = 1; i < lines.Length; i++)
        {
            var fields = lines[i].Split('|');
            var currency = new Currency
            {
                Date = DateTime.ParseExact(fields[0], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Name = fields[1],
                Amount = int.Parse(fields[2]),
                Rate = decimal.Parse(fields[3], CultureInfo.InvariantCulture)
            };
            currencies.Add(currency);
        }

        return currencies;
    }
}