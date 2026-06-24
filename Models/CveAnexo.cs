namespace TerminosApi.Models;

public class CveAnexo
{
    public int IdCatCveAnexos { get; set; }

    public string CveAnexoCodigo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string? Instancia { get; set; }

    public bool Activo { get; set; }
}