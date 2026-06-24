using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerminosApi.Data;
using TerminosApi.DTOs;
using TerminosApi.Models;
namespace TerminosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportesController(AppDbContext context)
    {
        _context = context;
    }



    [HttpGet("documento")]
    public async Task<IActionResult> GetDocumento([FromQuery] string folio)
    {
        var documento = await (
        from e in _context.Escritos
        join ec in _context.ElementCat
            on e.Tipo_escrito equals ec.Clave
        where e.Folio == folio
        select new
        {
            e.Folio,
            e.Expediente,
            e.Toca,

            e.Fecha_rec,
            e.hora_rec,

            e.Secretaria,
            e.Juzgado,
            e.Sala,

            e.Fojas,
            e.Traslados,

            e.Observ,

            TipoEscrito = ec.Descripcion,

            e.Id_usuario
        }
    ).FirstOrDefaultAsync();

        if (documento == null)
            return NotFound();

        var partes = await _context.PartesTer
            .Where(x => x.Folio == folio && x.Activo)
            .Select(x => x.Nombre)
            .ToListAsync();

        var anexosDb = await _context.AnexosTer
            .Where(x => x.Folio == folio && x.Activo)
            .ToListAsync();

        var catalogoAnexos = await _context.CveAnexos
            .ToListAsync();

        var anexos = (
            from a in anexosDb
            join c in catalogoAnexos
                on a.CveAnexo equals c.CveAnexoCodigo
            select new
            {
                Descripcion = c.Descripcion,
                a.Cantidad
            }
        ).ToList();
        var otrosAnexos = await _context.OtrosAnexos
        .Where(x => x.Folio == folio && x.Activo)
        .Select(x => x.Anexo)
        .ToListAsync();
        var quienRecibio = await _context.Usuarios
        .Where(x => x.Id == documento.Id_usuario.ToString())
        .Select(x => x.NombreUsuario)
        .FirstOrDefaultAsync();
        var nombreJuzgado = await _context.Juzgados
    .Where(x => x.Cve == documento.Juzgado)
    .Select(x => x.Descripcion)
    .FirstOrDefaultAsync();
        var nombreSala = await _context.Salas
        .Where(x => x.Cve == documento.Sala)
        .Select(x => x.Descripcion)
        .FirstOrDefaultAsync();
        var resultado = new DocumentoReporteDto
        {
            Folio = documento.Folio,
            Expediente = documento.Expediente,
            Toca = documento.Toca,

            FechaRecepcion = documento.Fecha_rec,
            HoraRecepcion = documento.hora_rec,

            Secretaria = documento.Secretaria,

            Juzgado = nombreJuzgado ?? documento.Juzgado,

            Sala = nombreSala ?? documento.Sala,

            Fojas = documento.Fojas,

            Traslados = documento.Traslados,

            Observaciones = documento.Observ,

            TipoEscrito = documento.TipoEscrito,

            QuienRecibio = quienRecibio,

            Partes = partes,

            Anexos = anexos
        .Select(x => new AnexoDto
        {
            Descripcion = x.Descripcion,
            Cantidad = x.Cantidad
        })
        .ToList(),

            OtrosAnexos = otrosAnexos
        };

        return Ok(resultado);
    }
    [HttpGet("conteo")]
    public async Task<IActionResult> ConteoPorInstancia(
    [FromQuery] string instancia)
    {
        var total = await _context.Escritos
            .Where(x => x.Instancia == instancia)
            .CountAsync();

        return Ok(new
        {
            Instancia = instancia,
            Total = total
        });
    }
    [HttpGet]
    public async Task<IActionResult> Buscar(
    [FromQuery] ReporteFiltroDto filtro)
    {
       var escritos = await ObtenerEscritosFiltrados(filtro, true);
        var tiposEscrito = await _context.ElementCat
            .ToDictionaryAsync(
                x => x.Clave,
                x => x.Descripcion
            );
        var juzgados = await _context.Juzgados
             .ToDictionaryAsync(
                x => x.Cve,
                x => x.Descripcion
     );
        var salas = await _context.Salas
            .ToListAsync();

        var idsUsuarios = escritos
    .Where(x => x.Id_usuario != null)
    .Select(x => x.Id_usuario!.ToString())
    .Distinct()
    .ToList();
        var usuarios = await _context.Usuarios
        .Where(x => x.Id != null &&
                    idsUsuarios.Contains(x.Id))
        .Select(x => new
        {
            x.Id,
            x.NombreUsuario
        })
        .ToListAsync();
        var resultado = escritos.Select(x => new ReporteResultadoDto
        {
            Folio = x.Folio,
            Expediente = x.Expediente,
            Toca = x.Toca,
            FechaRecepcion = x.Fecha_rec,

            TipoEscrito = tiposEscrito.ContainsKey(x.Tipo_escrito ?? "")
             ? tiposEscrito[x.Tipo_escrito!]
             : x.Tipo_escrito,
            Juzgado =
    x.Juzgado != null &&
    juzgados.ContainsKey(x.Juzgado)
        ? juzgados[x.Juzgado]
        : x.Juzgado,

            Sala = salas
    .Where(s => s.Cve == x.Sala)
    .Select(s => s.Descripcion)
    .FirstOrDefault() ?? x.Sala,
            QuienRecibio = usuarios
    .Where(u => u.Id == x.Id_usuario!.ToString())
    .Select(u => u.NombreUsuario)
    .FirstOrDefault(),

        }

        )
     .ToList();

        return Ok(resultado);
    }
  [HttpGet("pdf")]
public async Task<IActionResult> DescargarPdf(
    [FromQuery] ReporteFiltroDto filtro)
{
    var escritos = await ObtenerEscritosFiltrados(filtro, false);

    return Ok(new
    {
        Total = escritos.Count
    });
}

[HttpGet("excel")]
public async Task<IActionResult> DescargarExcel(
    [FromQuery] ReporteFiltroDto filtro)
{
    var escritos = await ObtenerEscritosFiltrados(filtro, false);

    return Ok(new
    {
        Total = escritos.Count
    });
}
private async Task<List<Escrito>> ObtenerEscritosFiltrados(
    ReporteFiltroDto filtro,
    bool limitarResultados = true)
{
    IQueryable<Escrito> query = _context.Escritos
        .FromSqlInterpolated($@"
            SELECT *
            FROM Escritos
            WHERE Instancia = {filtro.Instancia}
              AND CONVERT(datetime, Fecha_rec, 3)
                BETWEEN CONVERT(datetime, {filtro.FechaInicial}, 3)
                AND CONVERT(datetime, {filtro.FechaFinal}, 3)
        ");

    if (limitarResultados)
    {
        query = query.Take(100);
    }

    var escritos = await query.ToListAsync();

    if (!string.IsNullOrWhiteSpace(filtro.Folio))
    {
        escritos = escritos
            .Where(x =>
                x.Folio != null &&
                x.Folio.Contains(filtro.Folio))
            .ToList();
    }

    if (!string.IsNullOrWhiteSpace(filtro.Expediente))
    {
        escritos = escritos
            .Where(x =>
                x.Expediente != null &&
                x.Expediente.Contains(filtro.Expediente))
            .ToList();
    }

    if (!string.IsNullOrWhiteSpace(filtro.Juzgado))
    {
        escritos = escritos
            .Where(x => x.Juzgado == filtro.Juzgado)
            .ToList();
    }

    if (!string.IsNullOrWhiteSpace(filtro.Sala))
    {
        escritos = escritos
            .Where(x => x.Sala == filtro.Sala)
            .ToList();
    }

    if (!string.IsNullOrWhiteSpace(filtro.Usuario))
    {
        escritos = escritos
            .Where(x =>
                x.Id_usuario != null &&
                x.Id_usuario.ToString() == filtro.Usuario)
            .ToList();
    }

    return escritos;
}



}