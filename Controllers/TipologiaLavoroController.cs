using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using corsosharp.Data;
using corsosharp.Models;

namespace corsosharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipologiaLavoroController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TipologiaLavoroController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/tipologialavoro
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipologiaLavoro>>> GetAll()
    {
        var tipologie = await _context.TipologiaLavoro.ToListAsync();
        return Ok(tipologie);
    }

    // GET /api/tipologialavoro/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TipologiaLavoro>> GetById(Guid id)
    {
        var tipologia = await _context.TipologiaLavoro.FindAsync(id);

        if (tipologia == null)
            return NotFound();

        return Ok(tipologia);
    }

    // POST /api/tipologialavoro
    [HttpPost]
    public async Task<ActionResult<TipologiaLavoro>> Create([FromBody] TipologiaLavoro tipologia)
    {
        tipologia.CreatedAt = DateTime.Now;
        tipologia.UpdatedAt = DateTime.Now;

        _context.TipologiaLavoro.Add(tipologia);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = tipologia.Id }, tipologia);
    }

    // PUT /api/tipologialavoro/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<TipologiaLavoro>> Update(Guid id, [FromBody] TipologiaLavoro tipologia)
    {
        var existing = await _context.TipologiaLavoro.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Descrizione = tipologia.Descrizione;
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    // DELETE /api/tipologialavoro/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var tipologia = await _context.TipologiaLavoro.FindAsync(id);
        if (tipologia == null)
            return NotFound();

        _context.TipologiaLavoro.Remove(tipologia);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
