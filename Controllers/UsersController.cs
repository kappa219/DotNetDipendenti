using Microsoft.AspNetCore.Mvc;
using corsosharp.DTOs;
using corsosharp.Services;
using Microsoft.AspNetCore.Authorization;
using corsosharp.DB;

namespace corsosharp.Controllers;

[ApiController]  // Equivalente a @RestController in Spring
[Route("api/[controller]")]  // Route: /api/users (prende il nome del controlle)
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly DatabaseConnection _databaseConnection;

    // Dependency Injection (come @Autowired)
    public UsersController(IUserService userService, DatabaseConnection databaseConnection)
    {
        _userService = userService;
        _databaseConnection = databaseConnection;
    }

    // GET /api/users
    // Equivalente a @GetMapping
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);  // 200 OK
    }

    // GET /api/users/{guid}
    // Equivalente a @GetMapping("/{id}")
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();  // 404 Not Found

        return Ok(user);  // 200 OK
    }

    // POST /api/users
    // Equivalente a @PostMapping
  //  [Authorize(Roles = "Admin")] // Solo Admin può creare utenti
    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserDto dto)
    {

        var user = await _userService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            user
        );
    }

    // PUT /api/users/{guid}
    // Equivalente a @PutMapping("/{id}")
    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDto>> Update(Guid id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userService.UpdateAsync(id, dto);

        if (user == null)
            return NotFound();  // 404 Not Found

        return Ok(user);  // 200 OK
    }

    // DELETE /api/users/{guid}
    // Equivalente a @DeleteMapping("/{id}")
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _userService.DeleteAsync(id);

        if (!deleted)
            return NotFound();  // 404 Not Found

        return NoContent();  // 204 No Content (standard per DELETE)
    }

    // GET /api/users/email/test@test.com
    // Endpoint custom
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserResponseDto>> GetByEmail(string email)
    {
        var user = await _userService.GetByEmailAsync(email);

        if (user == null)
            return NotFound();

        return Ok(user);
    }


    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> Search([FromQuery] string? name = null, [FromQuery] string? email = null)
    {
        
             return Ok("");
      
    }
}
