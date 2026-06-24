namespace TerminosApi.DTOs;

public class DocumentoDetalleDto
{
    public string Folio { get; set; } = string.Empty;

    public string Expediente { get; set; } = string.Empty;

    public string Toca { get; set; } = string.Empty;

    public string Escrito { get; set; } = string.Empty;

    public string FechaRecepcion { get; set; } = string.Empty;

    public string HoraRecepcion { get; set; } = string.Empty;

    public string Secretaria { get; set; } = string.Empty;

    public string Traslados { get; set; } = string.Empty;

    public string Fojas { get; set; } = string.Empty;

    public string Observaciones { get; set; } = string.Empty;

    public string QuienCertifico { get; set; } = string.Empty;

    public List<string> Partes { get; set; } = [];

    public List<string> Anexos { get; set; } = [];

    public List<string> OtrosAnexos { get; set; } = [];
}