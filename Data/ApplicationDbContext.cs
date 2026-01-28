using Microsoft.EntityFrameworkCore;
using corsosharp.Models;

namespace corsosharp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Ogni DbSet<T> rappresenta una tabella nel database
        // Il nome della proprietà (Users) diventa il nome della tabella
        // T (Users) è il modello/entità che mappa le colonne
        public DbSet<Users> Users { get; set; }

        public DbSet<TipologiaLavoro> TipologiaLavoro { get; set; }

        public DbSet<AnagrafiaDipendente> AnagrafiaDipendente { get; set; }

        public DbSet<GiornataLavorativa> GiornateLavorative { get; set; }

        // Aggiungi altri DbSet quando crei i model:
        // public DbSet<Product> Products { get; set; }
        // public DbSet<Order> Orders { get; set; }
    }
}