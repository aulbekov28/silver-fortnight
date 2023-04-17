namespace Gmtq.Parser.Services.Abstractions;

public interface ICurrencyService
{
    Task LoadCurrencies(int year, CancellationToken token);
}