using Audit.EntityFramework;
using Infra.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Domain;
public class AutomationContext : AuditDbContext
{
    public AutomationContext(DbContextOptions<AutomationContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InformationEntityConfiguration());
    }
}
