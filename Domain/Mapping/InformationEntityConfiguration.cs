using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mapping;

public class InformationEntityConfiguration : IEntityTypeConfiguration<Information>
{
    public void Configure(EntityTypeBuilder<Information> builder)
    {
        builder.ToTable("COLUMNS", "INFORMATION_SCHEMA");

        builder.HasKey(i => new { i.TableName, i.ColumnName });

        builder.Property(i => i.TableName)
            .HasColumnName("TABLE_NAME")
            .IsRequired();

        builder.Property(i => i.ColumnName)
            .HasColumnName("COLUMN_NAME")
            .IsRequired();

        builder.Property(i => i.DataType)
            .HasColumnName("DATA_TYPE")
            .IsRequired();

        builder.Property(i => i.IsNullable)
            .HasColumnName("IS_NULLABLE")
            .IsRequired();

        builder.Property(i => i.MaxLength)
            .HasColumnName("CHARACTER_MAXIMUM_LENGTH");

        builder.Ignore(i => i.ColumnMap);
    }
}
