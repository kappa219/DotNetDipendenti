using System.ComponentModel.DataAnnotations;

namespace corsosharp.DTOs;

public class CreateAnagrafiaDipendenteDto
{
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Cognome { get; set; } = string.Empty;

    public int Eta { get; set; }

    public DateTime DataAssunzione { get; set; } = DateTime.Now;

    public DateTime? DataDimissione { get; set; }

    public decimal Stipendio { get; set; }

    public Guid? TipologiaLavoroId { get; set; }  // Solo l'ID, non l'oggetto!
}

public class UpdateAnagrafiaDipendenteDto
{
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Cognome { get; set; } = string.Empty;

    public int Eta { get; set; }

    public DateTime DataAssunzione { get; set; }

    public DateTime? DataDimissione { get; set; }

    public decimal Stipendio { get; set; }

    public Guid? TipologiaLavoroId { get; set; }
}
public class AnagrafiaDipendentiDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public int Eta { get; set; }
    public DateTime DataAssunzione { get; set; }
    public DateTime? DataDimissione { get; set; }
    public decimal Stipendio { get; set; }
    public TipologiaLavoroDto ? TipologiaLavoro { get; set; }
    public string? FotoUrl { get; set; }
}
