namespace TerminosApi.Models;

public class AnexoTer
{
    public int Idanexoter { get; set; }

    public string Folio { get; set; } = "";

    public string CveAnexo { get; set; } = "";

    public int Cantidad { get; set; }

    public bool Activo { get; set; }
}