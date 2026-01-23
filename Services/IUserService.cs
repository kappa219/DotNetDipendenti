using corsosharp.DTOs;

namespace corsosharp.Services;

// Interfaccia del Service (come in Java con Spring)
// Definisce il "contratto" - quali metodi deve avere il service
// Vantaggi:
// 1. Puoi avere implementazioni diverse (es: mock per i test)
// 2. Dependency Injection funziona meglio con le interfacce
// 3. Separa il "cosa" dal "come"

public interface IUserService
{
    // Ottieni tutti gli utenti
    Task<IEnumerable<UserResponseDto>> GetAllAsync();

    // Ottieni un utente per ID
    Task<UserResponseDto?> GetByIdAsync(Guid id);

    // Crea un nuovo utente
    Task<UserResponseDto> CreateAsync(CreateUserDto dto);

    // Aggiorna un utente esistente
    Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto);

    // Elimina un utente
    Task<bool> DeleteAsync(Guid id);

    // Cerca utente per email
    Task<UserResponseDto?> GetByEmailAsync(string email);
}
