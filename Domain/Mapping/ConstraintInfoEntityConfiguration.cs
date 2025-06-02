using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mapping;
public class ConstraintInfoEntityConfiguration : IEntityTypeConfiguration<ConstraintInfo>
{
    public void Configure(EntityTypeBuilder<ConstraintInfo> builder)
    {
        builder.ToTable("TABLE_CONSTRAINTS", "INFORMATION_SCHEMA");

        builder.HasKey(x => x.ConstraintName);

        builder.Property(x => x.ConstraintName).HasColumnName("CONSTRAINT_NAME");

        builder.Property(x => x.ConstraintType).HasColumnName("CONSTRAINT_TYPE");
    }
}