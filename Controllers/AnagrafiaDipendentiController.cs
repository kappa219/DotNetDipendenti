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

    // // // GET /api/anagrafiadipendenti/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AnagrafiaDipendente>> GetById(Guid id)
    {
        var dipendente = await _context.AnagrafiaDipendente
            .Include(d => d.TipologiaLavoro)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dipendente == null)
            return NotFound();

        return Ok(dipendente);
    }

    // // POST /api/anagrafiadipendenti
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AnagrafiaDipendente>> Create([FromBody] CreateAnagrafiaDipendenteDto dto)
    {
        var dipendente = new AnagrafiaDipendente
        {
            Nome = dto.Nome,
            Cognome = dto.Cognome,
            Eta = dto.Eta,
            DataAssunzione = dto.DataAssunzione,
            DataDimissione = dto.DataDimissione,
            Stipendio = dto.Stipendio,
            TipologiaLavoroId = dto.TipologiaLavoroId
        };

        _context.AnagrafiaDipendente.Add(dipendente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = dipendente.Id }, dipendente);
    }

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





    [HttpGet("dimessionati")]
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
            var command = new MySqlCommand("SELECT * FROM anagrafica_dipendente WHERE DataDimissione IS NOT NULL", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            //AnagrafiaDipendente dipendente;
          //  using (var reader = await command.ExecuteReaderAsync());
           // using var cmd = new MySqlCommand("elimina_utente", conn);
            //command.type = CommandType.Text;
            //command.CommandText= "SELECT * FROM anagrafica_dipendente WHERE DataAssunzione =@dataAssunzione";
            //command.Parameters.AddWithValue("@dataAssunzione", "2025-12-12"); // marammetrizzare la data 
            //MySqlDataReader reader = command.ExecuteReader();
            //il command ha diversi tipi di esecuzione a seconda di quello che voglio fare
            //ExecuteReader per le query che ritornano dei dati
            //ExecuteNonQuery per le query di tipo insert, update, delete che non ritornano dati
            //ExecuteScalar per le query che ritornano un singolo valore (es. un count)
            //qui uso ExecuteReader perch la query ritorna dei dati

            // una procedura posso anche definirla nel database e poi richiamarla da qui
            //ecco come fare    
            /*      
               /*     command.commandType = CommandType.StoredProcedure;
            
           in caso voglio passare dei parametri alla procedura
            command.Parameters.AddWithValue("@parametro1", valore1);
            int rows = await cmd.ExecuteNonQueryAsync();
             

                La procedura pu essere creata nel database cos:
                DELIMITER //
                CREATE PROCEDURE GetDimessi()
                BEGIN
                    SELECT * FROM anagrafica_dipendente WHERE DataDimissione IS NOT NULL;
                END //
                DELIMITER ; 





                */
            
            
            


            // var reader = command.ExecuteReader();





            //     while (reader.Read())
            //     {
                  
            //         // Leggo i campi nullable separatamente per evitare errori con DBNull
            //         DateTime? dataDimissione = null;
            //         if (!reader.IsDBNull(reader.GetOrdinal("DataDimissione")))
            //         {
            //             dataDimissione = reader.GetDateTime("DataDimissione");
            //         }

            //         Guid? tipologiaLavoroId = null;
            //         if (!reader.IsDBNull(reader.GetOrdinal("tipologia_lavoro_id")))
            //         {
            //             tipologiaLavoroId = reader.GetGuid("tipologia_lavoro_id");
            //         }

            //         var dipendente = new AnagrafiaDipendente
            //         {
            //             Id = reader.GetGuid("Id"),
            //             Nome = reader.GetString("Nome"),
            //             Cognome = reader.GetString("Cognome"),
            //             Eta = reader.GetInt32("Eta"),
            //             DataAssunzione = reader.GetDateTime("DataAssunzione"),
            //             DataDimissione = dataDimissione,
            //             Stipendio = reader.GetDecimal("Stipendio"),
            //             TipologiaLavoroId = tipologiaLavoroId
            //         };
            //         dipendenti.Add(dipendente);
            //         Console.WriteLine(dipendente.Nome);

                

          //  }


            return Ok(dipendenti);
        }
        finally
        {
            _dbConnection.CloseConnection(connection);
        }
    }
}