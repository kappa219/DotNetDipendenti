namespace corsosharp.DTOs;

public class GiornataLavorativaCreateDto
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
