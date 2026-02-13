using Microsoft.EntityFrameworkCore;
using corsosharp.Data;
using corsosharp.DTOs;
using corsosharp.Models;
using BCrypt.Net;

namespace corsosharp.Services;


 


public class AnagrafiaService
{
     private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
        private readonly ILogger<AnagrafiaService> _logger;

    public AnagrafiaService(ApplicationDbContext context, IConfiguration configuration, ILogger<AnagrafiaService> logger)
    {
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    public async Task<IEnumerable<AnagrafiaDipendentiDto>> GetAll()
    {
_logger.LogInformation("Recupero di tutti i dipendenti in corso...");
//_logger.LogDebug("Query SQL: SELECT * FROM anagrafica_dipendente");

       // DateTime oggi = new DateTime(2024, 6, 20);
        var dipendenti = await _context.AnagrafiaDipendente
            .Include(d => d.TipologiaLavoro)
           // .Where(d => d.DataDimissione == null)
            .ToListAsync();
  //questa ritorna tutti i dipendenti dopo una certa data
            // var esempio = await _context.AnagrafiaDipendente
            // .FromSql($"SELECT * FROM anagrafica_dipendente WHERE data_dimissione < {oggi}")
            // .OrderBy(d => d.DataDimissione)
            // .ToListAsync();

        return dipendenti.Select(d => MapToDto(d));
    }

    public async Task<AnagrafiaDipendente?> GetById(Guid id)
    {
        return await _context.AnagrafiaDipendente
            .Include(d => d.TipologiaLavoro)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<AnagrafiaDipendente> Create(AnagrafiaDipendente dipendente)
    {
        _context.AnagrafiaDipendente.Add(dipendente);
        await _context.SaveChangesAsync();
        return dipendente;
    }

    public async Task<AnagrafiaDipendente?> Update(Guid id, AnagrafiaDipendente datiAggiornati)
    {
        var existing = await _context.AnagrafiaDipendente.FindAsync(id);
        if (existing == null)
            return null;

        existing.Nome = datiAggiornati.Nome;
        existing.Cognome = datiAggiornati.Cognome;
        existing.Eta = datiAggiornati.Eta;
        existing.DataAssunzione = datiAggiornati.DataAssunzione;
        existing.DataDimissione = datiAggiornati.DataDimissione;
        existing.Stipendio = datiAggiornati.Stipendio;
        existing.TipologiaLavoroId = datiAggiornati.TipologiaLavoroId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(Guid id)
    {
        var dipendente = await _context.AnagrafiaDipendente.FindAsync(id);
        if (dipendente == null)
            return false;

        _context.AnagrafiaDipendente.Remove(dipendente);
        await _context.SaveChangesAsync();

        _logger.LogDebug("Dipendente eliminato: {DipendenteId}", id);
        return true;
    }







   
    private static AnagrafiaDipendentiDto MapToDto(AnagrafiaDipendente d)
    {
        return new AnagrafiaDipendentiDto
        {
            Id = d.Id,
            Nome = d.Nome,
            Cognome = d.Cognome,
            Eta = d.Eta,
            DataAssunzione = d.DataAssunzione,
            DataDimissione = d.DataDimissione,
           Stipendio = d.Stipendio,
            TipologiaLavoro = d.TipologiaLavoro != null ? new TipologiaLavoroDto
            {
                //Id = d.TipologiaLavoro.Id,
                Descrizione = d.TipologiaLavoro.Descrizione
            } : null
        };
    }
}