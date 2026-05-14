using corsosharp.Data;
using corsosharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace corsosharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UtentiEsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public UtentiEsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /api/utenties
    [HttpGet]
	    public async Task<ActionResult<IEnumerable<Utente>>> GetAll()
	    {
	        var utenti = await _db
	            .UtentiEs
	            .AsNoTracking()
	            .ToListAsync();

	        return Ok(utenti);
	    }

	    // DELETE /api/utenties/{id}
	    [HttpDelete("{id:guid}")]
	    public async Task<IActionResult> Delete(Guid id)
	    {
	        var utente = await _db.UtentiEs.FirstOrDefaultAsync(u => u.Id == id);
	        if (utente == null)
	        {
	            return NotFound();
	        }

	        _db.UtentiEs.Remove(utente);
	        await _db.SaveChangesAsync();

	        return NoContent();
	    }
	}
