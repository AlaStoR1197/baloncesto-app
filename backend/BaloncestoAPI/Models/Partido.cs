namespace BaloncestoAPI.Models;

public class Partido
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int MarcadorLocal { get; set; } = 0;
    public int MarcadorVisitante { get; set; } = 0;
    public int CuartoActual { get; set; } = 1;
    public bool Terminado { get; set; } = false;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    
    // Relaciones
    public int EquipoLocalId { get; set; }
    public Equipo EquipoLocal { get; set; } = null!;
    
    public int EquipoVisitanteId { get; set; }
    public Equipo EquipoVisitante { get; set; } = null!;
}