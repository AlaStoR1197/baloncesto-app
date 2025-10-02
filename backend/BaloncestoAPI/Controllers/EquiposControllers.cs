using Microsoft.AspNetCore.Mvc;
using BaloncestoAPI.Models;

namespace BaloncestoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private static List<Equipo> _equipos = new()
    {
        new Equipo { Id = 1, Nombre = "Lakers", Ciudad = "Los Angeles" },
        new Equipo { Id = 2, Nombre = "Warriors", Ciudad = "San Francisco" }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Equipo>> GetEquipos()
    {
        return Ok(_equipos);
    }

    [HttpGet("{id}")]
    public ActionResult<Equipo> GetEquipo(int id)
    {
        var equipo = _equipos.FirstOrDefault(e => e.Id == id);
        return equipo == null ? NotFound() : Ok(equipo);
    }

    [HttpPost]
    public ActionResult<Equipo> PostEquipo(Equipo equipo)
    {
        equipo.Id = _equipos.Max(e => e.Id) + 1;
        _equipos.Add(equipo);
        return CreatedAtAction(nameof(GetEquipo), new { id = equipo.Id }, equipo);
    }
}