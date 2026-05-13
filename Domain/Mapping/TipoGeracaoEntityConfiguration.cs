using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;
public class TipoGeracaoEntityConfiguration : IEntityTypeConfiguration<TipoGeracao>
{
    public void Configure(EntityTypeBuilder<TipoGeracao> builder)
    {
        builder.ToTable("Pragma_TipoGeracao");

        builder.HasKey(e => e.IdTipoGeracao);

        builder.Property(e => e.Tipo)
            .HasColumnName("Tipo")
            .HasColumnType("varchar")
            .IsRequired().HasMaxLength(50);

        builder.Property(e => e.DataInclusao)
            .HasColumnName("DataInclusao")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(e => e.IdOperadorInclusao)
            .HasColumnName("IdOperadorInclusao")
            .HasColumnType("int")
            .IsRequired();
    }
}