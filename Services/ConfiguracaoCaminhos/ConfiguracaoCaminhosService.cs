using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using System.Threading.Tasks;
using System.Collections.Generic;
using TCE.Base.Token;
using FluentValidation;
using System.IO;
using Domain.Enum;
using CrossCutting.Util;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ConfiguracaoCaminhosService : BaseService<ConfiguracaoCaminhos>, IConfiguracaoCaminhosService
{
    private readonly IConfiguracaoEstruturaProjetoService _configEstruturaService;
    public ConfiguracaoCaminhosService(IUnitOfWork uow, IConfiguracaoEstruturaProjetoService configEstruturaService) : base(uow)
    {
        _configEstruturaService = configEstruturaService;
    }

    public async Task<IEnumerable<ConfiguracaoCaminhos>> GetAllByOperadorAsync()
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        return await GetAllAsync(predicate: x => x.IdOperadorInclusao == int.Parse(tokenInfo.IdOperador), include: x => x.Include(y => y.ConfiguracaoEstruturaProjeto));
    }

    public async Task<ConfiguracaoCaminhos> GetByIdAsync(int id)
    {
        ConfiguracaoCaminhos configuracaoCaminhos = await GetByIdAsync(predicate: item => item.IdConfiguracaoCaminho == id);

        return configuracaoCaminhos;
    }

    public ConfiguracaoCaminhos Add(ConfiguracaoCaminhos configuracaoCaminhos)
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        configuracaoCaminhos.IdOperadorInclusao = int.Parse(tokenInfo.IdOperador);
        return base.Add<ConfiguracaoCaminhosValidator>(configuracaoCaminhos);
    }

    public ConfiguracaoCaminhos Update(ConfiguracaoCaminhos configuracaoCaminhos)
    {
        return base.Update<ConfiguracaoCaminhosValidator>(configuracaoCaminhos);
    }

    public async Task ValidateProjectStructure(string projectApiRootPath, string projectClientRootPath, int idEstruturaProjeto)
    {
        if (string.IsNullOrWhiteSpace(projectApiRootPath))
            throw new ValidationException("Caminho da API do projeto não pode ser vazia.");
        if (string.IsNullOrWhiteSpace(projectClientRootPath))
            throw new ValidationException("Caminho do Cliente do projeto não pode ser vazio.");

        ConfiguracaoEstruturaProjeto estruturaProjeto = await _configEstruturaService.GetByIdAsync(idEstruturaProjeto);

        if (estruturaProjeto == null)
            throw new ValidationException("Configuração de estrutura do projeto não pode ser vazia.");

        var requiredApiPaths = new[]
        {
            estruturaProjeto.ApiDependencyInjectionConfig,
            estruturaProjeto.ApiConfigureMap,
            estruturaProjeto.ApiControllers,
            estruturaProjeto.ApiEntities,
            estruturaProjeto.ApiMapping,
            estruturaProjeto.ApiContexts,
            estruturaProjeto.ApiServices,
        };

        var requiredClientPaths = new[]
        {
            estruturaProjeto.ClientServices,
            estruturaProjeto.ClientModels,
            estruturaProjeto.ClientModulos,
            estruturaProjeto.ClientArquivoRotas,
        };

        foreach (var relativePath in requiredApiPaths)
        {
            var fullPath = Path.Combine(projectApiRootPath, relativePath);
            EnsurePathExists(fullPath, TemplateType.Api);
        }

        foreach (var relativePath in requiredClientPaths)
        {
            var fullPath = Path.Combine(projectClientRootPath, relativePath);
            EnsurePathExists(fullPath, TemplateType.Client);
        }

    }

    private static void EnsurePathExists(string fullPath, TemplateType templateType)
    {
        if (File.Exists(fullPath) || Directory.Exists(fullPath))
            return;

        string tipoProjeto = templateType switch
        {
            TemplateType.Api => "Api",
            TemplateType.Client => "Client",
            _ => "Desconhecido"
        };

        string tipoAlvo = Path.HasExtension(fullPath) ? "Arquivo" : "Diretório";

        throw new ValidationException(
            $"Estrutura inválida no caminho de destino {tipoProjeto}. {tipoAlvo.Capitalize()} obrigatório não encontrado: {fullPath}"
        );
    }
}