using Gmtq.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Gmtq.Data;

public class CurrencyContext : DbContext 
{
    public CurrencyContext(
        DbContextOptions<CurrencyContext> options) : base(options)
    {
    }
    
    public DbSet<Currency> Currencies { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => new { e.Date, e.Name });
        });
    }
}