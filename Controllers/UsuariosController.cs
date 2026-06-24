using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerminosApi.Data;
using TerminosApi.DTOs;

namespace TerminosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("con-escritos")]
public async Task<IActionResult> GetUsuariosConEscritos()
{
    var usuarios = await (
        from e in _context.Escritos
        join u in _context.Usuarios
            on e.Id_usuario.ToString() equals u.Id
        where e.Activo && u.Activo
        select new UsuarioDto
        {
              Id = u.Id,
    Nombre = u.NombreUsuario.Trim()
        }
    )
    .Distinct()
    .OrderBy(x => x.Nombre)
    .ToListAsync();

    return Ok(usuarios);
}
}