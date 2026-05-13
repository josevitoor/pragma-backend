using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;
public class WorkspaceGeracaoEntityConfiguration : IEntityTypeConfiguration<WorkspaceGeracao>
{
    public void Configure(EntityTypeBuilder<WorkspaceGeracao> builder)
    {
        builder.ToTable("Pragma_WorkspaceGeracao");

        builder.HasKey(e => e.IdWorkspaceGeracao);


        builder.Property(e => e.IdTipoGeracao)
            .HasColumnName("IdTipoGeracao")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.Nome)
            .HasColumnName("Nome")
            .HasColumnType("varchar")
            .IsRequired().HasMaxLength(150);

        builder.Property(e => e.Arquivo)
            .HasColumnName("Arquivo")
            .HasColumnType("nvarchar")
            .IsRequired();

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