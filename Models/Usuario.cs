namespace TerminosApi.Models;

public class Usuario
{
    public string Id { get; set; } = string.Empty;

    public string NombreUsuario { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public string Tipo { get; set; } = string.Empty;
}