using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using corsosharp.Data;
using corsosharp.Models;
using corsosharp.DTOs;
using corsosharp.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using corsosharp.DB;
using MySqlConnector;


namespace corsosharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnagrafiaDipendentiController : ControllerBase

{
    private AnagrafiaService _anagrafiaService;
    private readonly ApplicationDbContext _context;
    private readonly DatabaseConnection _dbConnection;

    public AnagrafiaDipendentiController(ApplicationDbContext context, AnagrafiaService anagrafiaService, DatabaseConnection dbConnection)
    {
        _context = context;
        _anagrafiaService = anagrafiaService;
        _dbConnection = dbConnection;
    }

    // GET /api/anagrafiadipendenti
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnagrafiaDipendente>>> GetAll()
    {
        var dipendenti = await _anagrafiaService.GetAll();
        return Ok(dipendenti);
    }

    // // GET /api/anagrafiadipendenti/{id}
    // [HttpGet("{id:guid}")]
    // public async Task<ActionResult<AnagrafiaDipendente>> GetById(Guid id)
    // {
    //     var dipendente = await _context.AnagrafiaDipendente
    //         .Include(d => d.TipologiaLavoro)
    //         .FirstOrDefaultAsync(d => d.Id == id);

    //     if (dipendente == null)
    //         return NotFound();

    //     return Ok(dipendente);
    // }

    // POST /api/anagrafiadipendenti
    // [HttpPost]
    // [Authorize(Roles = "Admin")]
    // public async Task<ActionResult<AnagrafiaDipendente>> Create([FromBody] CreateAnagrafiaDipendenteDto dto)
    // {
    //     var dipendente = new AnagrafiaDipendente
    //     {
    //         Nome = dto.Nome,
    //         Cognome = dto.Cognome,
    //         Eta = dto.Eta,
    //         DataAssunzione = dto.DataAssunzione,
    //         DataDimissione = dto.DataDimissione,
    //         Stipendio = dto.Stipendio,
    //         TipologiaLavoroId = dto.TipologiaLavoroId
    //     };

    //     _context.AnagrafiaDipendente.Add(dipendente);
    //     await _context.SaveChangesAsync();

    //     return CreatedAtAction(nameof(GetById), new { id = dipendente.Id }, dipendente);
    // }

    // PUT /api/anagrafiadipendenti/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<AnagrafiaDipendente>> Update(Guid id, [FromBody] UpdateAnagrafiaDipendenteDto dto)
    {
        var existing = await _context.AnagrafiaDipendente.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Nome = dto.Nome;
        existing.Cognome = dto.Cognome;
        existing.Eta = dto.Eta;
        existing.DataAssunzione = dto.DataAssunzione;
        existing.DataDimissione = dto.DataDimissione;
        existing.Stipendio = dto.Stipendio;
        existing.TipologiaLavoroId = dto.TipologiaLavoroId;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    // DELETE /api/anagrafiadipendenti/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var dipendente = await _context.AnagrafiaDipendente.FindAsync(id);
        if (dipendente == null)
            return NotFound();

        _context.AnagrafiaDipendente.Remove(dipendente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpGet("custom-query")]
    public async Task<ActionResult<IEnumerable<AnagrafiaDipendente>>> GetCustomQuery()
    {
        var connection = _dbConnection.GetConnection();
        if (connection == null)
        {
            return StatusCode(500, "Errore nella connessione al database.");
        }

        try
        {

            var dipendenti = new List<AnagrafiaDipendente>();
            var command = new MySqlCommand("SELECT * FROM anagrafica_dipendente WHERE DataAssunzione IS NOT NULL", connection);
            {
                var reader = command.ExecuteReader();
                {
                    while (reader.Read())
                    {
                        // var dip=new AnagrafiaDipendente(  reader.GetGuid("Id"),
                        //                             reader.GetString("Nome"),
                        //                              reader.GetString("Cognome"),
                        //                             reader.GetInt32("Eta"),
                        //                             reader.GetDateTime("DataAssunzione"),
                        //                           reader.GetDateTime("DataDimissione"),
                        //                              reader.GetDecimal("Stipendio"),
                        //                               reader.GetGuid("tipologia_lavoro_id")
                        // );


                        var dipendente = new AnagrafiaDipendente
                        {  //
                            Id = reader.GetGuid("Id"),
                            Nome = reader.GetString("Nome"),
                            Cognome = reader.GetString("Cognome"),
                            Eta = reader.GetInt32("Eta"),
                            DataAssunzione = reader.GetDateTime("DataAssunzione"),
                            DataDimissione = reader.GetDateTime("DataDimissione"),
                            Stipendio = reader.GetDecimal("Stipendio"),
                            TipologiaLavoroId = reader.GetGuid("tipologia_lavoro_id")
                        };
                        dipendenti.Add(dipendente);
                        Console.WriteLine(dipendente.Nome);

                    }

                }


                return Ok(dipendenti);
            }
        finally
        {
            _dbConnection.CloseConnection(connection);
        }
    }
}