namespace TerminosApi.DTOs;

public class ReportePdfDto
{
    public string Instancia { get; set; } = string.Empty;

    public string? Juzgado { get; set; }

    public string? Sala { get; set; }

    public string FechaInicial { get; set; } = string.Empty;

    public string FechaFinal { get; set; } = string.Empty;

    public int TotalRegistros { get; set; }

    public List<DocumentoReporteDto> Documentos { get; set; } = [];
}