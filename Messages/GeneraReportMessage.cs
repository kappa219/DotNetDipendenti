namespace Contracts.Messages;

public class GeneraReportMessage
{
    public string Tipo { get; set; } = string.Empty; // "dipendente" o "annuale"
    public string? UserId { get; set; }
    public string? ConnectionId { get; set; }
    public string? NomeDipendente { get; set; }
    public int? Anno { get; set; }
    public List<GiornataLavorativaDto> Giornate { get; set; } = new();
}

public class GiornataLavorativaDto
{
    public Guid DipendenteId { get; set; }
    public DateTime Data { get; set; }
    public decimal OreLavorate { get; set; }
    public string Azienda { get; set; } = string.Empty;
    public string Mansione { get; set; } = string.Empty;
    public int Ora { get; set; }
    public string? Note { get; set; }
    public string? TitoloNota { get; set; }
    public TimeOnly OraInizio { get; set; }
    public TimeOnly OraFine { get; set; }
}
