using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gmtq.Data.Models;

public class Currency
{
    public DateTime Date { get; set; }

    public string Name { get; set; }

    public int Amount { get; set; }

    public decimal Rate { get; set; }
}