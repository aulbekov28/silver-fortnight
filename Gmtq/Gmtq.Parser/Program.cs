using Gmtq.Data;
using Gmtq.Parser.Models;
using Gmtq.Parser.Services;
using Gmtq.Parser.Services.Abstractions;
using Gmtq.Parser.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    
    .ConfigureServices((builder, services) =>
    {
        var loadConfig = new CurrencyLoadingConfig();
        if (args.Length == 0)
        {
            if (int.TryParse(args[0].Trim(), out var year))
            {
                loadConfig = new CurrencyLoadingConfig { Year = year};
            }
        }

        services.AddDbContext<CurrencyContext>(optionActions =>
            optionActions.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")!));
        services.AddSingleton(loadConfig);
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddHostedService<CurrencyLoader>();
    })
    .RunConsoleAsync();