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
    .Property(x => x.NombreUsuario)
    .HasColumnName("Usuario");

        modelBuilder.Entity<Escrito>()
            .ToTable("Escritos");

        modelBuilder.Entity<Escrito>()
            .HasKey(x => x.IdEscritos);
    }

}