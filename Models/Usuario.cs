namespace TerminosApi.Models;

public class Usuario
{
    public string? Id { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Pass { get; set; }

    public bool Activo { get; set; }

    public string? Tipo { get; set; }
}