namespace TerminosApi.Models;

public class OtroAnexo
{
    public int IdOtros_anexos { get; set; }

    public string Folio { get; set; } = string.Empty;

    public string Anexo { get; set; } = string.Empty;

    public bool Activo { get; set; }
}