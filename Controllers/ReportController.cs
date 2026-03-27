using Microsoft.AspNetCore.Mvc;
using corsosharp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace corsosharp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly GiornateLavorativeServices _giornateService;
    private readonly ReportClientService _reportClient;
    private readonly ILogger<ReportController> _logger;

    public ReportController(
        GiornateLavorativeServices giornateService,
        ReportClientService reportClient,
        ILogger<ReportController> logger)
    {
        _giornateService = giornateService;
        _reportClient = reportClient;
        _logger = logger;
    }

    // GET /api/report/dipendente/{id}/{nomeDipendente}
    // Pubblica messaggio su RabbitMQ per generare Excel del dipendente
    [HttpGet("dipendente/{id}/{nomeDipendente}")]
    public async Task<IActionResult> GeneraExcelDipendente(Guid id, string nomeDipendente)
    {
        _logger.LogInformation("Richiesta Excel per dipendente {Id} - {Nome}", id, nomeDipendente);

        var giornate = await _giornateService.giornatedipentente(id);

        if (giornate.Count == 0)
            return NotFound("Nessuna giornata trovata per questo dipendente");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var connectionId = Request.Headers["X-SignalR-ConnectionId"].FirstOrDefault();
        await _reportClient.PubblicaReportDipendenteAsync(giornate, nomeDipendente, userId, connectionId);

        return Accepted(new { messaggio = "Report in elaborazione, verrà salvato a breve." });
    }

    // GET /api/report/annuale/{anno}
    // Pubblica messaggio su RabbitMQ per generare Excel annuale
    //[Authorize(Roles = "Admin")]
    [HttpGet("annuale/{anno}")]
    public async Task<IActionResult> GeneraExcelAnnuale(int anno)
    {
        _logger.LogInformation("Richiesta Excel annuale per anno {Anno}", anno);

        var tutteLeGiornate = await _giornateService.Allgiornate();
        var giornateAnno = tutteLeGiornate
            .Where(g => g.Data.Year == anno)
            .ToList();

        if (giornateAnno.Count == 0)
            return NotFound($"Nessuna giornata trovata per l'anno {anno}");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var connectionId = Request.Headers["X-SignalR-ConnectionId"].FirstOrDefault();
        await _reportClient.PubblicaReportAnnualeAsync(giornateAnno, anno, userId, connectionId);

        return Accepted(new { messaggio = $"Report annuale {anno} in elaborazione, verrà salvato a breve." });
    }
}
