using Microsoft.EntityFrameworkCore;
using BaloncestoAPI.Models;

namespace BaloncestoAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<Jugador> Jugadores { get; set; }
    public DbSet<Partido> Partidos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurar relaciones para Partidos
        modelBuilder.Entity<Partido>()
            .HasOne(p => p.EquipoLocal)
            .WithMany(e => e.PartidosLocal)
            .HasForeignKey(p => p.EquipoLocalId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Partido>()
            .HasOne(p => p.EquipoVisitante)
            .WithMany(e => e.PartidosVisitante)
            .HasForeignKey(p => p.EquipoVisitanteId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Configurar nombre de tablas
        modelBuilder.Entity<Equipo>().ToTable("equipos");
        modelBuilder.Entity<Jugador>().ToTable("jugadores");
        modelBuilder.Entity<Partido>().ToTable("partidos");
    }
}