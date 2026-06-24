using Microsoft.EntityFrameworkCore;
using TerminosApi.Models;

namespace TerminosApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Juzgado> Juzgados { get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Escrito> Escritos { get; set; }

public DbSet<ElementCat> ElementCat { get; set; }

public DbSet<ParteTer> PartesTer { get; set; }

public DbSet<AnexoTer> AnexosTer { get; set; }

public DbSet<CatAnexo> CatAnexos { get; set; }

public DbSet<OtroAnexo> OtrosAnexos { get; set; }

public DbSet<CveAnexo> CveAnexos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Juzgado>()
            .ToTable("cveJuzgado");

        modelBuilder.Entity<Juzgado>()
            .HasKey(x => x.IdCveJuzgado);
        modelBuilder.Entity<Sala>()
            .ToTable("cveSala");

        modelBuilder.Entity<Sala>()
            .HasKey(x => x.IdCveSalas);
        modelBuilder.Entity<Usuario>()
    .ToTable("Usuarios");

modelBuilder.Entity<Usuario>()
    .HasKey(x => x.Id);

modelBuilder.Entity<Usuario>()
    .Property(x => x.NombreUsuario)
    .HasColumnName("Usuario");

        modelBuilder.Entity<Escrito>()
            .ToTable("Escritos");

        modelBuilder.Entity<Escrito>()
            .HasKey(x => x.IdEscritos);
            modelBuilder.Entity<ElementCat>()
    .ToTable("elemen_cat");

modelBuilder.Entity<ElementCat>()
    .HasKey(x => x.Idelemencat);

modelBuilder.Entity<ParteTer>()
    .ToTable("PartesTer");

modelBuilder.Entity<ParteTer>()
    .HasKey(x => x.IdPartesTer);

modelBuilder.Entity<AnexoTer>()
    .ToTable("AnexosTer");

modelBuilder.Entity<AnexoTer>()
    .HasKey(x => x.Idanexoter);

modelBuilder.Entity<CatAnexo>()
    .ToTable("CATANEXOS_borrar");

modelBuilder.Entity<CatAnexo>()
    .HasKey(x => x.Clave);

modelBuilder.Entity<CatAnexo>()
    .Property(x => x.Clave)
    .HasColumnName("CLAVE");

modelBuilder.Entity<CatAnexo>()
    .Property(x => x.Des)
    .HasColumnName("DES");

modelBuilder.Entity<OtroAnexo>()
    .ToTable("Otros_anexos");

modelBuilder.Entity<OtroAnexo>()
    .HasKey(x => x.IdOtros_anexos);

    modelBuilder.Entity<CveAnexo>()
    .ToTable("cveAnexos");

modelBuilder.Entity<CveAnexo>()
    .HasKey(x => x.IdCatCveAnexos);

modelBuilder.Entity<CveAnexo>()
    .Property(x => x.CveAnexoCodigo)
    .HasColumnName("cveAnexo");
    }
    

}