using Microsoft.AspNetCore.Mvc;
using corsosharp.Models;

namespace corsosharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GemmaController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GemmaController> _logger;
    private const string OllamaUrl = "http://localhost:11434/api/chat";
    private static readonly System.Text.Json.JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public GemmaController(IHttpClientFactory httpClientFactory, ILogger<GemmaController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Generate([FromBody] GemmaRequestDto dto)
    {
        _logger.LogInformation("Richiesta a Gemma2: {Prompt}", dto.Prompt);

        var client = _httpClientFactory.CreateClient();

        var payload = new OllamaChatRequest();
        payload.Messages.AddRange(dto.Storico);
        payload.Messages.Add(new OllamaMessage { Role = "user", Content = dto.Prompt });

        var response = await client.PostAsJsonAsync(OllamaUrl, payload);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Ollama ha risposto con {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode, "Errore da Ollama");
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<OllamaResponseDto>(json, JsonOptions);
        var risposta = result?.Message?.Content ?? string.Empty;
        return Ok(new { risposta });
    }
}
