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

    public AnagrafiaService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<IEnumerable<AnagrafiaDipendentiDto>> GetAll()
    {
        var dipendenti = await _context.AnagrafiaDipendente
            .Include(d => d.TipologiaLavoro)
           // .Where(d => d.DataDimissione == null)
            .ToListAsync();

        return dipendenti.Select(d => MapToDto(d));
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