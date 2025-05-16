using Domain.Entities;
using Infra.Mapping;
using Microsoft.EntityFrameworkCore;

public class DynamicDbContext : DbContext
{
    public DynamicDbContext(DbContextOptions<DynamicDbContext> options) : base(options) { }

    public DbSet<Information> Informations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InformationEntityConfiguration());
    }
}