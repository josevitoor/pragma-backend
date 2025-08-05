using Domain.Entities;
using FluentValidation;

namespace Services;

public class ConfiguracaoConexaoBancoValidator : AbstractValidator<ConfiguracaoConexaoBanco>
{
    public ConfiguracaoConexaoBancoValidator()
    {
        RuleFor(p => p.BaseDados)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(p => p.Usuario)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(p => p.Senha)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(p => p.Servidor)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(p => p.Porta)
            .NotNull()
            .GreaterThan(0);

        RuleFor(p => p.DataInclusao)
            .NotNull();

        RuleFor(p => p.IdOperadorInclusao)
            .NotNull();

        RuleFor(p => p.IdSessao)
            .NotNull();
    }
}
