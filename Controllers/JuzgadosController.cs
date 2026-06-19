using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerminosApi.Data;

namespace TerminosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JuzgadosController : ControllerBase
{
    private readonly AppDbContext _context;

    public JuzgadosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var juzgados = await _context.Juzgados
            .Where(x => x.Activo)
            .OrderBy(x => x.Descripcion)
            .ToListAsync();

        return Ok(juzgados);
    }

    [HttpGet("instancia/{instancia}")]
    public async Task<IActionResult> GetPorInstancia(string instancia)
    {
        var juzgados = await _context.Juzgados
            .Where(x =>
                x.Activo &&
                x.Instancia == instancia)
            .OrderBy(x => x.Descripcion)
            .ToListAsync();

        return Ok(juzgados);
    }
}