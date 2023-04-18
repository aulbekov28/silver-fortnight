using Gmtq.Data.Models;

namespace Gmtq.Web.Models.Responses;

public class CurrencyRateResponse
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    public decimal Rate { get; set; }

    public static CurrencyRateResponse FromCurrency(Currency currency)
    {
        return new CurrencyRateResponse
        {
            Date = currency.Date,
            Name = currency.Name,
            Amount = currency.Amount,
            Rate = currency.Rate
        };
    }
}