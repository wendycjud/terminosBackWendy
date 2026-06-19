using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerminosApi.Data;

namespace TerminosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalasController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var salas = await _context.Salas
            .Where(x => x.Activo)
            .OrderBy(x => x.Descripcion)
            .ToListAsync();

        return Ok(salas);
    }

    [HttpGet("instancia/{instancia}")]
    public async Task<IActionResult> GetPorInstancia(string instancia)
    {
        var salas = await _context.Salas
            .Where(x =>
                x.Activo &&
                x.Instancia == instancia)
            .OrderBy(x => x.Descripcion)
            .ToListAsync();

        return Ok(salas);
    }
}