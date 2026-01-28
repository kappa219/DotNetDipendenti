using Microsoft.EntityFrameworkCore;
using corsosharp.Data;
using corsosharp.DTOs;
using corsosharp.Models;
using BCrypt.Net;

namespace corsosharp.Services;

// Implementazione del Service - qui c'è la logica di business
// Equivalente a @Service in Spring Boot

public class UserService : IUserService
{
    // Dependency Injection del DbContext (come @Autowired in Java)
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL - equivalente a findAll() in Spring
    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();

        //non restituisco la password 
        return users.Select(user => MapToDto(user)).OrderByDescending(u => u.CreatedAt);
    }

    // GET BY ID  
    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return null;

        return MapToDto(user);
    }

    // CREATE - equivalente a save()
    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        // Crea l'entità dal DTO
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new Users
        
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = hashedPassword,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Role=dto.Role
        };

        // Aggiunge al context (come entityManager.persist())
        _context.Users.Add(user);

        // Salva nel database
        await _context.SaveChangesAsync();

        return MapToDto(user);
    }

    // UPDATE - equivalente a save() su entità esistente
    public async Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return null;

        // Aggiorna solo i campi forniti (update parziale)
        if (dto.Name != null)
            user.Name = dto.Name;

        if (dto.Email != null)
            user.Email = dto.Email;

        if (dto.Password != null)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Password = hashedPassword;
        }
        user.UpdatedAt = DateTime.Now;

        // SaveChanges rileva automaticamente le modifiche (dirty checking)
        await _context.SaveChangesAsync();

        return MapToDto(user);
    }

    // DELETE - equivalente a deleteById()
    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    // FIND BY EMAIL - query personalizzata
    public async Task<UserResponseDto?> GetByEmailAsync(string email)
    {
        // LINQ = equivalente a @Query o Criteria API in Java
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null;

        return MapToDto(user);
    }

    // Helper per mappare Entity -> DTO
    private static UserResponseDto MapToDto(Users user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,     
            Role = user.Role
        };
    }
}
