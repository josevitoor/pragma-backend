using Domain.Entities;
using FluentValidation;

namespace Services;

public class ConfiguracaoCaminhosValidator : AbstractValidator<ConfiguracaoCaminhos>
{
    public ConfiguracaoCaminhosValidator()
    {

        RuleFor(p => p.CaminhoApi).NotEmpty().MaximumLength(500);

        RuleFor(p => p.CaminhoCliente).NotEmpty().MaximumLength(500);

        RuleFor(p => p.IdConfiguracaoEstrutura).NotEmpty();

        RuleFor(p => p.DataInclusao).NotNull();

        RuleFor(p => p.IdOperadorInclusao).NotNull();

        RuleFor(p => p.IdSessao).NotNull();

    }
}