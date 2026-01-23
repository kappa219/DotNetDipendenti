using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace corsosharp.Models;

[Table("users")]  // Nome tabella nel DB (come @Table in Java)
public class Users
{
    [Key]  // Chiave primaria (come @Id in Java)
    public Guid Id { get; set; } = Guid.NewGuid();  // UUID generato automaticamente

    [Required]  // NOT NULL (come @NotNull)
    [MaxLength(100)]  // VARCHAR(100) (come @Column(length=100))
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [EmailAddress]  // Validazione formato email
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("Role")]
    public string Role { get; set; } = string.Empty;

    [Column("created_at")] 
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}