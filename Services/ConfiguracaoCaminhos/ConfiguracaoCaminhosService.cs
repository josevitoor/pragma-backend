using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using System.Threading.Tasks;
using System.Collections.Generic;
using TCE.Base.Token;
using FluentValidation;
using System.IO;
using Domain.Enum;
using System.Linq;
using CrossCutting.Util;
using System.Text.RegularExpressions;

namespace Services;

public class ConfiguracaoCaminhosService : BaseService<ConfiguracaoCaminhos>, IConfiguracaoCaminhosService
{
    public ConfiguracaoCaminhosService(IUnitOfWork uow) : base(uow)
    {
    }

    public async Task<IEnumerable<ConfiguracaoCaminhos>> GetAllByOperadorAsync()
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        return await GetAllAsync(predicate: x => x.IdOperadorInclusao == int.Parse(tokenInfo.IdOperador));
    }

    public async Task<ConfiguracaoCaminhos> GetByIdAsync(int id)
    {
        ConfiguracaoCaminhos configuracaoCaminhos = await base.GetByIdAsync(predicate: item => item.IdConfiguracaoCaminho == id);

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

    public void ValidateProjectStructure(string projectApiRootPath, string projectClientRootPath, string routerFilePath)
    {
        if (string.IsNullOrWhiteSpace(projectApiRootPath))
            throw new ValidationException("Caminho da API do projeto não pode ser vazia.");
        if (string.IsNullOrWhiteSpace(projectApiRootPath))
            throw new ValidationException("Caminho do Cliente do projeto não pode ser vazio.");
        if (string.IsNullOrWhiteSpace(routerFilePath))
            throw new ValidationException("Caminho do arquivo de rotas não pode ser vazio.");

        var requiredApiPaths = new[]
        {
            "Api\\Configuration\\DependencyInjectionConfig.cs",
            "Api\\AutoMapper\\ConfigureMap.cs",
            "Api\\Controllers",
            "Domain\\Entities",
            "Domain\\Mapping",
            "Contexts",
            "Services"
        };

        var requiredClientPaths = new[]
        {
            "src\\app\\app.module.ts",
            "src\\app\\services",
            "src\\app\\models",
            "src\\app\\modulos",
            "angular.json"
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

        EnsureRouterContainsInsertPoint(routerFilePath);
    }

    private static void EnsurePathExists(string fullPath, TemplateType templateType)
    {
        if (File.Exists(fullPath) || Directory.Exists(fullPath))
            return;

        if (fullPath.Contains("Contexts"))
        {
            var domainContextsPath = fullPath.Replace("Contexts", "Domain\\Contexts");
            if (Directory.Exists(domainContextsPath))
            {
                var contextFile = Directory.GetFiles(domainContextsPath, "*Context.cs").FirstOrDefault();
                if (contextFile != null)
                    return;
            }

            var infraContextPath = fullPath.Replace("Contexts", "Infra\\Contexts");
            if (Directory.Exists(infraContextPath))
            {
                var contextFile = Directory.GetFiles(infraContextPath, "*Context.cs").FirstOrDefault();
                if (contextFile != null)
                    return;
            }

            throw new ValidationException($"Estrutura inválida no caminho de destino API. Arquivo obrigatório não encontrado: {fullPath}.*Context.cs");
        }

        string tipoProjeto = templateType switch
        {
            TemplateType.Api => "API",
            TemplateType.Client => "Client",
            _ => "Desconhecido"
        };

        string tipoAlvo = Path.HasExtension(fullPath) ? "Arquivo" : "Diretório";

        throw new ValidationException(
            $"Estrutura inválida no caminho de destino {tipoProjeto}. {tipoAlvo.Capitalize()} obrigatório não encontrado: {fullPath}"
        );
    }

    private static void EnsureRouterContainsInsertPoint(string routerFilePath)
    {
        if (!File.Exists(routerFilePath))
            throw new ValidationException($"Arquivo de rotas não encontrado em: {routerFilePath}");

        var content = File.ReadAllText(routerFilePath);

        var has404 = Regex.IsMatch(content, @"{ path:\s*['""]404['""]");
        var hasWildcard = Regex.IsMatch(content, @"{ path:\s*['""]\*\*['""]");
        var hasDashboardChildren = Regex.IsMatch(content, @"path:\s*'dashboard'.*children:\s*\[", RegexOptions.Singleline);

        if (!has404 && !hasWildcard && !hasDashboardChildren)
            throw new ValidationException(
                $"O arquivo de rotas '{routerFilePath}' não segue o padrão esperado. Adicione um ponto de inserção válido (rota 404, wildcard '**' ou children dentro de 'dashboard')."
            );
    }
}