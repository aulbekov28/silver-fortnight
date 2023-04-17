using Gmtq.Parser.Models;
using Gmtq.Parser.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gmtq.Parser.Worker;

public class CurrencyLoader : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CurrencyLoader> _logger;
    private readonly CurrencyLoadingConfig _currencyLoadingConfig;

    public CurrencyLoader(IServiceProvider serviceProvider,
        ILogger<CurrencyLoader> logger,
        CurrencyLoadingConfig currencyLoadingConfig)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _currencyLoadingConfig = currencyLoadingConfig;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading currencies started");
        using var scope = _serviceProvider.CreateScope();
        var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();

        await currencyService.LoadCurrencies(_currencyLoadingConfig.Year, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading currencies ended");
        return Task.CompletedTask;
    }
}