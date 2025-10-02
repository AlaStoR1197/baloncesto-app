namespace BaloncestoAPI.Models;

public class Jugador
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string? Posicion { get; set; }
    public decimal? Estatura { get; set; }
    public int? Edad { get; set; }
    public string? Nacionalidad { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    
    // Relación con Equipo
    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;
}