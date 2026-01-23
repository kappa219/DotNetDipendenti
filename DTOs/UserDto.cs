namespace corsosharp.DTOs;

// DTO = Data Transfer Object
// Servono per separare il modello del database da ciò che esponi via API.
// Non esponi mai direttamente l'entità (es: non vuoi mostrare la password!)

// DTO per la creazione di un utente (quello che ricevi dal client)
public class CreateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}

// DTO per l'aggiornamento (opzionale: permette update parziali)
public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

// DTO per la risposta (quello che restituisci al client - SENZA password!)
public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
