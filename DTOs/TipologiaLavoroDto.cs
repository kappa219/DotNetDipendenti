using System.Text.Json.Serialization;

public class TipologiaLavoroDto
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
}

public class GetTipologiaLavoroDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
}