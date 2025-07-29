using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mapping;

public class ConfiguracaoGeracaoEntityConfiguration : IEntityTypeConfiguration<ConfiguracaoGeracao>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoGeracao> builder)
    {
        builder.ToTable("Pragma_ConfiguracaoGeracao");

        builder.HasKey(e => e.IdConfiguracao);

        builder.Property(e => e.IdConfiguracao)
               .HasColumnName("IdConfiguracao");

        builder.Property(e => e.BaseDados)
               .HasColumnName("BaseDados")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Usuario)
               .HasColumnName("Usuario")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(e => e.Senha)
               .HasColumnName("Senha")
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(e => e.Servidor)
               .HasColumnName("Servidor")
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(e => e.Porta)
               .HasColumnName("Porta")
               .IsRequired();

        builder.Property(e => e.CaminhoApi)
               .HasColumnName("CaminhoApi")
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.CaminhoCliente)
               .HasColumnName("CaminhoCliente")
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.CaminhoArquivoRota)
               .HasColumnName("CaminhoArquivoRota")
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.DataInclusao)
               .HasColumnName("DataInclusao")
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.IdOperadorInclusao)
               .HasColumnName("IdOperadorInclusao")
               .IsRequired();

        builder.Property(e => e.IdSessao)
               .HasColumnName("IdSessao")
               .IsRequired();
    }
}