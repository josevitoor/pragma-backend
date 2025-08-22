using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ConfiguracaoEstruturaProjetoEntityConfiguration : IEntityTypeConfiguration<ConfiguracaoEstruturaProjeto>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoEstruturaProjeto> builder)
    {
        builder.ToTable("Pragma_ConfiguracaoEstruturaProjetos");

        builder.HasKey(e => e.IdConfiguracaoEstrutura);

        builder.Property(e => e.NomeEstrutura)
            .HasColumnName("NomeEstrutura")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(50);

        builder.Property(e => e.ApiDependencyInjectionConfig)
            .HasColumnName("ApiDependencyInjectionConfig")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

        builder.Property(e => e.ApiConfigureMap)
            .HasColumnName("ApiConfigureMap")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

        builder.Property(e => e.ApiControllers)
            .HasColumnName("ApiControllers")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

        builder.Property(e => e.ApiEntities)
            .HasColumnName("ApiEntities")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(500);

        builder.Property(e => e.ApiMapping)
            .HasColumnName("ApiMapping")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

        builder.Property(e => e.ApiContexts)
            .HasColumnName("ApiContexts")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

        builder.Property(e => e.ApiServices)
            .HasColumnName("ApiServices")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

        builder.Property(e => e.ApiImportBaseService)
            .HasColumnName("ApiImportBaseService")
            .HasColumnType("nvarchar")
            .HasMaxLength(50);

        builder.Property(e => e.ApiImportUOW)
            .HasColumnName("ApiImportUOW")
            .HasColumnType("nvarchar")
            .HasMaxLength(50);

        builder.Property(e => e.ApiImportPaginate)
            .HasColumnName("ApiImportPaginate")
            .HasColumnType("nvarchar")
            .HasMaxLength(50);

        builder.Property(e => e.ClientServices)
            .HasColumnName("ClientServices")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

        builder.Property(e => e.ClientModels)
            .HasColumnName("ClientModels")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

        builder.Property(e => e.ClientModulos)
            .HasColumnName("ClientModulos")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(500);

        builder.Property(e => e.ClientArquivoRotas)
            .HasColumnName("ClientArquivoRotas")
            .HasColumnType("nvarchar")
            .IsRequired().HasMaxLength(100);

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