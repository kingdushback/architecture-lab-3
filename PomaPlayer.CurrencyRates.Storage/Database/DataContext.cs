using Microsoft.EntityFrameworkCore;
using PomaPlayer.CurrencyRates.Storage.Models;

namespace PomaPlayer.CurrencyRates.Storage.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<ReportDaily> Reports { get; set; }
}
