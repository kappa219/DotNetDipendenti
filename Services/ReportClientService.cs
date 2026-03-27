using MassTransit;
using Contracts.Messages;
using corsosharp.Models;

namespace corsosharp.Services;

public class ReportClientService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ReportClientService> _logger;

    public ReportClientService(IPublishEndpoint publishEndpoint, ILogger<ReportClientService> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PubblicaReportDipendenteAsync(List<GiornataLavorativa> giornate, string nomeDipendente, string? userId = null, string? connectionId = null)
    {
        _logger.LogInformation("Pubblicazione messaggio report dipendente {Nome}", nomeDipendente);

        var messaggio = new GeneraReportMessage
        {
            Tipo = "dipendente",
            UserId = userId,
            ConnectionId = connectionId,
            NomeDipendente = nomeDipendente,
            Giornate = giornate.Select(g => new GiornataLavorativaDto
            {
                DipendenteId = g.DipendenteId,
                Data = g.Data,
                OreLavorate = g.OreLavorate,
                Azienda = g.Azienda,
                Mansione = g.Mansione,
                Ora = g.Ora,
                Note = g.Note,
                TitoloNota = g.TitoloNota,
                OraInizio = g.OraInizio,
                OraFine = g.OraFine
            }).ToList()
        };

        await _publishEndpoint.Publish(messaggio);
    }

    public async Task PubblicaReportAnnualeAsync(List<GiornataLavorativa> giornate, int anno, string? userId = null, string? connectionId = null)
    {
        _logger.LogInformation("Pubblicazione messaggio report annuale {Anno}", anno);

        var messaggio = new GeneraReportMessage
        {
            Tipo = "annuale",
            UserId = userId,
            ConnectionId = connectionId,
            Anno = anno,
            Giornate = giornate.Select(g => new GiornataLavorativaDto
            {
                DipendenteId = g.DipendenteId,
                Data = g.Data,
                OreLavorate = g.OreLavorate,
                Azienda = g.Azienda,
                Mansione = g.Mansione,
                Ora = g.Ora,
                Note = g.Note,
                TitoloNota = g.TitoloNota,
                OraInizio = g.OraInizio,
                OraFine = g.OraFine
            }).ToList()
        };

        await _publishEndpoint.Publish(messaggio);//glielo passa alla coda di MassTransit che lo consegna al servizio di generazione report
    }
}
