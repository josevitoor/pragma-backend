using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mapping;
public class TableConstraintEntityConfiguration : IEntityTypeConfiguration<TableConstraint>
{
    public void Configure(EntityTypeBuilder<TableConstraint> builder)
    {
        builder.ToTable("CONSTRAINT_COLUMN_USAGE", "INFORMATION_SCHEMA");

        builder.HasKey(i => new { i.TableName, i.ColumnName });

        builder.Property(x => x.TableName).HasColumnName("TABLE_NAME");

        builder.Property(x => x.ColumnName).HasColumnName("COLUMN_NAME");

        builder.Property(x => x.ConstraintName).HasColumnName("CONSTRAINT_NAME");

        builder.HasOne(tc => tc.Information)
            .WithOne(i => i.TableConstraint)
            .HasForeignKey<TableConstraint>(tc => new { tc.TableName, tc.ColumnName });

        builder.HasOne(tc => tc.ConstraintInfo)
               .WithMany()
               .HasForeignKey(tc => tc.ConstraintName);
    }
}