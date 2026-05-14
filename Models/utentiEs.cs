using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace corsosharp.Models;

[Table("UtentiEs")] // 
public class Utente
{
    [Key]  public Guid Id { get; set; } = Guid.NewGuid(); 
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Indirizzo { get; set; } = string.Empty;
    public int Eta { get; set; }
    public string Password { get; set; } = string.Empty;
}
