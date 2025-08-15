using Domain.Entities;
using Infra.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class DynamicDbContext : DbContext
{
    public DynamicDbContext(DbContextOptions<DynamicDbContext> options) : base(options) { }

    public DbSet<Information> Informations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InformationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoEstruturaProjetoEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoCaminhosEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TableConstraintEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ConstraintInfoEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoConexaoBancoEntityConfiguration());
    }
}