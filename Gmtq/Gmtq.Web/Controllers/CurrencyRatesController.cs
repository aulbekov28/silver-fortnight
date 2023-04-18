using System.Net;
using Gmtq.Web.Models.Responses;
using Gmtq.Web.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Gmtq.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyRatesController : ControllerBase
{
    private readonly ICurrencyRateService _currencyRateService;

    public CurrencyRatesController(
        ICurrencyRateService currencyRateService)
    {
        _currencyRateService = currencyRateService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCurrencyRates(CancellationToken token)
    {
        var currencyNames = await _currencyRateService.GetCurrencyNames(token);
        return Ok(currencyNames);
    }
    
    [HttpGet("{currencyName}/{date}")]
    [ProducesResponseType(typeof(CurrencyRateResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(string currencyName, DateTime date, CancellationToken token)
    {
        var currency = await _currencyRateService.GetCurrencyRate(currencyName, date, token);
        if (currency is null)
        {
            return NotFound();
        }
        return Ok(CurrencyRateResponse.FromCurrency(currency));
    }
}