using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace corsosharp.Models;

[Table("tipologia_lavoro")]
public class TipologiaLavoro
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [MaxLength(40)]
    public string Descrizione { get; set; } = string.Empty;
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


}