namespace corsosharp.Models;

// Messaggio singolo della conversazione (role: "user" o "assistant")
public class OllamaMessage
{
    [System.Text.Json.Serialization.JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

// Quello che manda il client al nostro controller
public class GemmaRequestDto
{
    public string Prompt { get; set; } = string.Empty;
    public List<OllamaMessage> Storico { get; set; } = [];
}

// Quello che mandiamo a Ollama /api/chat
public class OllamaChatRequest
{
    [System.Text.Json.Serialization.JsonPropertyName("model")]
    public string Model { get; set; } = "gemma2:2b";

    [System.Text.Json.Serialization.JsonPropertyName("messages")]
    public List<OllamaMessage> Messages { get; set; } = new()
    {
        // new() { Role = "system", Content = "Sei Pierre, un francese esagerato che parla SOLO italiano mescolato con parole francesi. ..." }
        // new() { Role = "system", Content = "Sei un assistente esperto di animali e insetti. ..." }
        // new() { Role = "system", Content = "fai il riassunto breve e dettagliato del testo fornito" }
        new()
        {
            Role = "system",
            Content = "Sei il massimo esperto in carte di Pokémon e sui Pokémon in generale, e anche in carte Magic."
        }
    };

    

    [System.Text.Json.Serialization.JsonPropertyName("stream")]
    public bool Stream { get; set; } = true;
}

// Quello che risponde Ollama /api/chat
public class OllamaResponseDto
{
    [System.Text.Json.Serialization.JsonPropertyName("message")]
    public OllamaMessage? Message { get; set; }
}
