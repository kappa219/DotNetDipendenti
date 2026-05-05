using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using corsosharp.Models;
using System.Net.Http.Json;
using System.Text;

namespace corsosharp.Controllers;

[Authorize]
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

        using var request = new HttpRequestMessage(HttpMethod.Post, OllamaUrl)
        {
            Content = JsonContent.Create(payload)
        };

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Ollama ha risposto con {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode, "Errore da Ollama");
        }

        // Ollama con stream=true restituisce JSON line-delimited (un oggetto JSON per riga).
        // Accumuliamo i chunk di testo per ottenere una risposta completa.
        var rispostaBuilder = new StringBuilder();
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                var chunk = System.Text.Json.JsonSerializer.Deserialize<OllamaResponseDto>(line, JsonOptions);
                if (!string.IsNullOrEmpty(chunk?.Message?.Content))
                {
                    rispostaBuilder.Append(chunk.Message.Content);
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogWarning(ex, "Chunk JSON non valido da Ollama: {Chunk}", line);
            }
        }

        var risposta = rispostaBuilder.ToString();
        return Ok(new { risposta });
    }
}
