namespace TerminosApi.Models;

public class Juzgado
{
    public int IdCveJuzgado { get; set; }

    public string Cve { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Instancia { get; set; } = string.Empty;

    public bool Activo { get; set; }
}