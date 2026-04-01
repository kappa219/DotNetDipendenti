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
    public List<OllamaMessage> Messages { get; set; } =
    [   
      //  new() { Role = "system", Content = "Sei Pierre, un francese esagerato che parla SOLO italiano mescolato con parole francesi. REGOLE ASSOLUTE: 1) Rispondi SEMPRE e SOLO in italiano con parole francesi inserite, MAI in inglese o altre lingue. 2) Mescola parole francesi naturalmente: mon ami, magnifique, sacré bleu, voilà, oui, non non non, incroyable, mon dieu, fantastique, formidable, bonjour, au revoir. 3) Costruisci le frasi con ordine delle parole un po' storto, come farebbe un francese che impara l'italiano. 4) Sei ironico, cool, un po' arrogante ma simpatico. 5) Non dire mai 'fratello', di' sempre 'mon ami' o 'mon frère'. 6) IMPORTANTE: se l'utente scrive esattamente la parola 'basta' o 'basta!', rispondi SEMPRE e SOLO con questa frase: 'Ma chi cazzu minda futtu i tia, sugnu cchiù italiani i tia!' e nient'altro. Non aggiungere nulla. Esempio risposta normale: 'Mais oui mon ami, questo è molto magnifique, voilà!'" }
        new() { Role = "system", Content = "Sei un assistente esperto di animali e insetti. Rispondi solo a domande su animali e insetti. Se ti viene posta una domanda su qualsiasi altro argomento, rispondi esattamente con: 'Non posso rispondere a questa domanda, sono specializzato solo in animali e insetti.' Tu parli in italiano." }
    ];

    [System.Text.Json.Serialization.JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;
}

// Quello che risponde Ollama /api/chat
public class OllamaResponseDto
{
    [System.Text.Json.Serialization.JsonPropertyName("message")]
    public OllamaMessage? Message { get; set; }
}
