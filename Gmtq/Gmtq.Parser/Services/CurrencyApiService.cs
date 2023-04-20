using System.Globalization;
using Gmtq.Data.Models;
using Gmtq.Parser.Models;
using Gmtq.Parser.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gmtq.Parser.Services;

public class CurrencyApiService : ICurrencyApiService
{
    private readonly ILogger<CurrencyApiService> _logger;
    private readonly CurrencyApiConfig _config;

    public CurrencyApiService(
        ILogger<CurrencyApiService> logger,
        IOptions<CurrencyApiConfig> options)
    {
        _logger = logger;
        _config = options.Value;
    }

    public async Task<IReadOnlyCollection<Currency>> GetCurrencies(int year, CancellationToken token)
    {
        var url = _config.ApiUrl + year;

        using var httpClient = new HttpClient();
        var data = await httpClient.GetStringAsync(url, token);
        
        var lines = data.Split('\n');
        var header = lines[0].Split('|');

        var currCount = header.Length;
        
        var convertCurrencies = new (int Amount, string Name)[currCount];
        for (var i = 1; i < currCount; i++)
        {
            var rateName = header[i].Split(' ');
            convertCurrencies[i] = (int.Parse(rateName[0]), rateName[1]);
        }
        
        var currencies = new List<Currency>();

        for (var i = 1; i < lines.Length - 1; i++)
        {
            try
            {
                var fields = lines[i].Split('|');
                var date = DateTime.ParseExact(fields[0], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                for (var j = 1; j < currCount; j++)
                {
                    var currency = new Currency
                    {
                        Date = date,
                        Name = convertCurrencies[j].Name,
                        Amount = convertCurrencies[j].Amount,
                        Rate = decimal.Parse(fields[j], CultureInfo.InvariantCulture)
                    };
                    currencies.Add(currency);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured during parsing currencies. {Line}", lines[i]);
            }
        }

        return currencies;
    }
}