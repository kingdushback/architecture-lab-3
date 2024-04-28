using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PomaPlayer.CurrencyRates.Storage.Models;

public sealed class ReportDaily
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid IsnReportDaily { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public string Currency { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public decimal Rate { get; set; }
}
