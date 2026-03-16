using Microsoft.AspNetCore.Mvc;
using corsosharp.Services;
using corsosharp.Models;
using corsosharp.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Data;

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

   // [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiornataLavorativa>>> GetAll()
    {
        var giornate = await _giornatelavorative.Allgiornate();

        return Ok(giornate);
    }
   //qui cosa devo fare perche se  c'è data inizio e data fine mi deve filtrare per settimana altrimenti mi deve restituire tutte le giornate del dipendente per costruire il file exel
   // non so se è corretto mettere due if uno dentro l'altro oppure se è meglio mettere un if con una condizione che verifica se data inizio e data fine sono null o no
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<GiornataLavorativa>>> GetByDipendente(
        Guid id, [FromQuery] DateTime? dataInizio, [FromQuery] DateTime? dataFine)
    {
        _logger.LogInformation("Recupero giornate per dipendente con ID: {DipendenteId} con data da {DataInizio} a {DataFine}", id, dataInizio, dataFine);
        if (dataInizio == null || dataFine == null)
        {
            _logger.LogInformation("Recupero giornate senza filtro per costruire file EXEL : {DipendenteId}", id);
            var giornate = await _giornatelavorative.giornatedipentente(id);
            return Ok(giornate);
        } 
        else
        {   

        // var giornate = await _giornatelavorative.giornatedipentente(id );
         _logger.LogInformation("Recupero giornate con filtro di data per dipendente con ID: {DipendenteId} da {DataInizio} a {DataFine} per file exel", id, dataInizio, dataFine);
        var giornatesettimanali = await _giornatelavorative.giornatedipententeforweek(id, dataInizio, dataFine);

        return Ok(giornatesettimanali);

        }
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
            TitoloNota = dto.TitoloNota,
            OraInizio = dto.OraInizio,
            OraFine = dto.OraFine
        };
        _logger.LogInformation("Inserimento giornata per dipendente con ID: {DipendenteId}", dto.DipendenteId);
        var messaggio = await _giornatelavorative.insertGiornata(giornata);
        return Ok(messaggio);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGiornata(Guid id)
    {
        _logger.LogError("Giornata eliminata: {GiornataId}", id);
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
            TitoloNota = dto.TitoloNota,
            OraInizio = dto.OraInizio,
            OraFine = dto.OraFine
        };
        var messaggio = await _giornatelavorative.updateGiornata(id, giornata);
        _logger.LogInformation("Giornata aggiornata: {GiornataId} , {Giornata}", id, giornata);
        //_logger.LogInformation("Giornata aggiornata: {GiornataId}, Dipendente: {DipendenteId}, Data: {Data}",id, giornata.DipendenteId, giornata.Data);

        return Ok(messaggio);
    }

}