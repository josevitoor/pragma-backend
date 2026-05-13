using Domain.Entities;
using FluentValidation;

namespace Services;
public class WorkspaceGeracaoValidator : AbstractValidator<WorkspaceGeracao>
{
    public WorkspaceGeracaoValidator()
    {

        RuleFor(p => p.IdTipoGeracao).NotNull();

        RuleFor(p => p.Nome).NotEmpty().MaximumLength(150);

        RuleFor(p => p.Arquivo).NotEmpty().MaximumLength(-1);

        RuleFor(p => p.DataInclusao).NotNull();

        RuleFor(p => p.IdOperadorInclusao).NotNull();
    }
}