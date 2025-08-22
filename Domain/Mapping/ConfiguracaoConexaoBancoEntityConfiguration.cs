using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mapping;

public class ConfiguracaoConexaoBancoEntityConfiguration : IEntityTypeConfiguration<ConfiguracaoConexaoBanco>
{
       public void Configure(EntityTypeBuilder<ConfiguracaoConexaoBanco> builder)
       {
              builder.ToTable("Pragma_ConfiguracaoConexaoBanco");

              builder.HasKey(e => e.IdConfiguracaoConexaoBanco);

              builder.Property(e => e.IdConfiguracaoConexaoBanco)
                     .HasColumnName("IdConfiguracaoConexaoBanco");

              builder.Property(e => e.BaseDados)
                     .HasColumnName("BaseDados")
                     .HasMaxLength(50)
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
                     .HasMaxLength(50)
                     .IsRequired();

              builder.Property(e => e.Porta)
                     .HasColumnName("Porta")
                     .IsRequired();

              builder.Property(e => e.DataInclusao)
                     .HasColumnName("DataInclusao")
                     .HasColumnType("datetime")
                     .IsRequired();

              builder.Property(e => e.IdOperadorInclusao)
                     .HasColumnName("IdOperadorInclusao")
                     .IsRequired();
       }
}