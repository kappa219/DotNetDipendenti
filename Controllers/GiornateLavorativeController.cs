using Microsoft.AspNetCore.Mvc;
using corsosharp.Services;
using corsosharp.Models;
using corsosharp.DTOs;

namespace corsosharp.Controllers;

[ApiController]
[Route("api/[controller]")]

public class GiornateLavorativeController : ControllerBase
{
    private readonly GiornateLavorativeServices _giornatelavorative;
       private readonly ILogger<GiornateLavorativeController> _logger;

    public GiornateLavorativeController(GiornateLavorativeServices giornateLavorative, ILogger<GiornateLavorativeController> logger)
    {
        _giornatelavorative = giornateLavorative;
        _logger = logger;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiornataLavorativa>>> GetAll()
    {
        var giornate = await _giornatelavorative.Allgiornate();
        return Ok(giornate);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<GiornataLavorativa>>> GetByDipendente(Guid id)
    {
        var giornate = await _giornatelavorative.giornatedipentente(id);
        return Ok(giornate);
    }
    [HttpPost]
    public async Task<IActionResult> insertEvent(GiornataLavorativaCreateDto dto)
    {
        var giornata = new GiornataLavorativa
        {
            DipendenteId = dto.DipendenteId,
            Data = dto.Data,
            OreLavorate = dto.OreLavorate,
            Azienda = dto.Azienda,
            Mansione = dto.Mansione,
            Ora = dto.Ora,
            Note = dto.Note,
            TitoloNota = dto.TitoloNota
        };
        var messaggio = await _giornatelavorative.insertGiornata(giornata);
        return Ok(messaggio);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGiornata(Guid id)
    {
        var messaggio = await _giornatelavorative.deleteGiornata(id);
        return Ok(messaggio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGiornata(Guid id, GiornataLavorativaCreateDto dto)
    {
        var giornata = new GiornataLavorativa
        {
            DipendenteId = dto.DipendenteId,
            Data = dto.Data,
            OreLavorate = dto.OreLavorate,
            Azienda = dto.Azienda,
            Mansione = dto.Mansione,
            Ora = dto.Ora,
            Note = dto.Note,
            TitoloNota = dto.TitoloNota
        };
        var messaggio = await _giornatelavorative.updateGiornata(id, giornata);
        _logger.LogInformation("Giornata aggiornata: {GiornataId}", id);
        return Ok(messaggio);
    }

}