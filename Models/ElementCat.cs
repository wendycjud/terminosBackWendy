namespace TerminosApi.Models;

public class ElementCat
{
    public int Idelemencat { get; set; }

    public string Clave { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string? Instancia { get; set; }

    public bool Activo { get; set; }
}