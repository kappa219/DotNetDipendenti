using corsosharp.DTOs;
using corsosharp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using corsosharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace corsosharp.Services;

public class GiornateLavorativeServices
{
    private readonly ApplicationDbContext _context;

    public GiornateLavorativeServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GiornataLavorativa>> Allgiornate()
    {

        var giornate = await _context.GiornateLavorative.ToListAsync();

        return giornate;



    }
    public async Task<List<GiornataLavorativa>> giornatedipentente(Guid id)
    {

        var giornate = await _context.GiornateLavorative.Where(g => g.DipendenteId == id)
        .OrderByDescending(giornate => giornate.Data)
        .ToListAsync();

        return giornate;

    }


    public async Task<string> insertGiornata(GiornataLavorativa giornata)
    {
        _context.GiornateLavorative.Add(giornata);
        await _context.SaveChangesAsync();
        return "giornata aggiunta";
    }




    public async Task<string> deleteGiornata(Guid idGiornata)
    {
        var giornata = await _context.GiornateLavorative.FindAsync(idGiornata);
        if (giornata == null)
        {
            return "giornata non trovata";
        }
        _context.GiornateLavorative.Remove(giornata);
        await _context.SaveChangesAsync();
        return "giornata eliminata";
    }

    public async Task<string> updateGiornata(Guid idGiornata, GiornataLavorativa giornataAggiornata)
    {
        var giornata = await _context.GiornateLavorative.FindAsync(idGiornata);
        if (giornata == null)
        {
            return "giornata non trovata";
        }

        giornata.Data = giornataAggiornata.Data;
        giornata.OreLavorate = giornataAggiornata.OreLavorate;
        giornata.Azienda = giornataAggiornata.Azienda;
        giornata.Mansione = giornataAggiornata.Mansione;
        giornata.Ora = giornataAggiornata.Ora;
        giornata.Note = giornataAggiornata.Note;
        giornata.TitoloNota = giornataAggiornata.TitoloNota;

        await _context.SaveChangesAsync();
        return "giornata aggiornata";
    }

}