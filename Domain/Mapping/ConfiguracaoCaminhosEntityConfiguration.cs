using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ConfiguracaoCaminhosEntityConfiguration : IEntityTypeConfiguration<ConfiguracaoCaminhos>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoCaminhos> builder)
    {
        builder.ToTable("Pragma_ConfiguracaoCaminhos");

        builder.HasKey(e => e.IdConfiguracaoCaminho);


        builder.Property(e => e.CaminhoApi)
            .HasColumnName("CaminhoApi")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(500);

        builder.Property(e => e.CaminhoCliente)
            .HasColumnName("CaminhoCliente")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(500);

        builder.Property(e => e.IdConfiguracaoEstrutura)
            .HasColumnName("IdConfiguracaoEstrutura")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.DataInclusao)
            .HasColumnName("DataInclusao")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(e => e.IdOperadorInclusao)
            .HasColumnName("IdOperadorInclusao")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.IdSessao)
            .HasColumnName("IdSessao")
            .HasColumnType("int")
            .IsRequired();


        builder.HasOne(e => e.ConfiguracaoEstruturaProjeto)
            .WithMany(p => p.ConfiguracaoCaminhos)
            .HasForeignKey(e => e.IdConfiguracaoEstrutura)
            .IsRequired();
    }
}