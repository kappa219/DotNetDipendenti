using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace corsosharp.Models;

[Table("giornate_lavorative")]
public class GiornataLavorativa
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("dipendente_id")]
    public Guid DipendenteId { get; set; }

    [Required]
    public DateTime Data { get; set; }

    [Column("ore_lavorate")]
    public decimal OreLavorate { get; set; }

    [MaxLength(100)]
    public string Azienda { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Mansione { get; set; } = string.Empty;
    [Required]
    public int Ora { get; set; }


    [MaxLength(500)]
    public string? Note { get; set; }
    public string ? TitoloNota{get;set;}

    // Navigation property
    [ForeignKey("DipendenteId")]
    public AnagrafiaDipendente? Dipendente { get; set; }
}
