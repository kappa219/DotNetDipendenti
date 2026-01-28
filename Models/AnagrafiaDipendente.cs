using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace corsosharp.Models;
[Table("anagrafica_dipendente")]
public class AnagrafiaDipendente
{

    public AnagrafiaDipendente()
    {
    }
    [Key]  // Chiave primaria (come @Id in Java)
    public Guid Id { get; set; } = Guid.NewGuid();  // UUID generato automaticamente

    [Required] 
    [MaxLength(100)] 
    public string Nome { get; set; } = string.Empty;
    [Required] 
    [MaxLength(100)] 
    public string Cognome { get; set; } = string.Empty;
    [MaxLength(30) ]
public string Indirizzo { get; set; } = string.Empty;
    public int Eta { get; set;  }   
    [Required]
    public DateTime DataAssunzione { get; set; } 
    public DateTime? DataDimissione { get; set; }
    public decimal Stipendio { get; set;  }
    [Column("tipologia_lavoro_id")]
    public Guid? TipologiaLavoroId { get; set;  }

    [ForeignKey("TipologiaLavoroId")]
    public TipologiaLavoro? TipologiaLavoro { get; set; }

    // (relazione 1-N)
    public ICollection<GiornataLavorativa> GiornateLavorative { get; set; } = new List<GiornataLavorativa>();
}