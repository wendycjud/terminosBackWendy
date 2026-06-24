namespace TerminosApi.Models;

public class Escrito
{
    public int IdEscritos { get; set; }

    public string Instancia { get; set; } = "";

    public string Folio { get; set; } = "";

    public string? Fecha_rec { get; set; } = "";

    public string? hora_rec { get; set; } = "";

    public string? Tipo_escrito { get; set; } = "";

    public string? Toca { get; set; } = "";

    public string? Sala { get; set; } = "";

    public string? Expediente { get; set; } = "";

    public string? Secretaria { get; set; }

    public string? Juzgado { get; set; }

    public string? Fojas { get; set; } = "";

    public string? Traslados { get; set; } = "";

    public int? Id_usuario { get; set; }

    public string? Observ { get; set; }

    public bool Activo { get; set; }
}