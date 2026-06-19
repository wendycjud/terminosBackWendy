namespace TerminosApi.Models;

public class Usuario
{
    public int Id { get; set; }

    public string NombreUsuario  { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public string Tipo { get; set; } = string.Empty;
}