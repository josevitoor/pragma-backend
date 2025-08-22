using Domain.Entities;
using FluentValidation;

namespace Services;

public class ConfiguracaoEstruturaProjetoValidator : AbstractValidator<ConfiguracaoEstruturaProjeto>
{
    public ConfiguracaoEstruturaProjetoValidator()
    {

        RuleFor(p => p.NomeEstrutura).NotEmpty().MaximumLength(50);

        RuleFor(p => p.ApiDependencyInjectionConfig).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ApiConfigureMap).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ApiControllers).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ApiEntities).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ApiMapping).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ApiContexts).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ApiServices).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ApiImportBaseService).MaximumLength(50);

        RuleFor(p => p.ApiImportUOW).MaximumLength(50);

        RuleFor(p => p.ApiImportPaginate).MaximumLength(50);

        RuleFor(p => p.ClientServices).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ClientModels).NotEmpty().MaximumLength(100);

        RuleFor(p => p.ClientModulos).NotEmpty().MaximumLength(100);

        RuleFor(p => p.DataInclusao).NotNull();

        RuleFor(p => p.IdOperadorInclusao).NotNull();

    }
}