namespace TerminosApi.DTOs;

public class DocumentoReporteDto
{
    public string? Folio { get; set; }

    public string? Expediente { get; set; }

    public string? Toca { get; set; }

    public string? FechaRecepcion { get; set; }

    public string? HoraRecepcion { get; set; }

    public string? Secretaria { get; set; }

    public string? Juzgado { get; set; }

    public string? Sala { get; set; }

    public string? Fojas { get; set; }

    public string? Traslados { get; set; }

    public string? Observaciones { get; set; }

    public string? TipoEscrito { get; set; }

   public string? QuienRecibio { get; set; }

    public List<string> Partes { get; set; } = [];

    public List<AnexoDto> Anexos { get; set; } = [];

    public List<string> OtrosAnexos { get; set; } = [];
}