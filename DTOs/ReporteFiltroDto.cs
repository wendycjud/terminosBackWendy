namespace TerminosApi.DTOs;

public class ReporteFiltroDto
{
    public string Instancia { get; set; } = string.Empty;

    public string FechaInicial { get; set; } = string.Empty;

    public string FechaFinal { get; set; } = string.Empty;

    public string? Folio { get; set; }

    public string? Expediente { get; set; }

    public string? Juzgado { get; set; }

    public string? Sala { get; set; }

    public string? Usuario { get; set; }
}