using Audit.EntityFramework;
using Infra.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class PragmaContext : AuditDbContext
{
    public PragmaContext(DbContextOptions<PragmaContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InformationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TableConstraintEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ConstraintInfoEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoConexaoBancoEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoCaminhosEntityConfiguration());
    }
}
