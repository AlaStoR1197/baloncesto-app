namespace BaloncestoAPI.Models;

public class Equipo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ciudad { get; set; }
    public string? Logo { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    
    // Relaciones
    public List<Jugador> Jugadores { get; set; } = new();
    public List<Partido> PartidosLocal { get; set; } = new();
    public List<Partido> PartidosVisitante { get; set; } = new();
}