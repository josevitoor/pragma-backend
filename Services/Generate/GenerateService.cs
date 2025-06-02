using System.Threading.Tasks;
using Scriban;
using System.IO;
using Domain.Filter;
using System.Text.RegularExpressions;
using FluentValidation;
using System.Linq;
using Domain.Enum;
using CrossCutting.Util;

namespace Services;

public class GenerateService : IGenerateService
{
    private const string ApiTemplatesDirectory = "Templates\\Automation\\BackendTemplates";
    private const string ClientTemplatesDirectory = "Templates\\Automation\\FrontendTemplates";
    private readonly IInformationService _informationService;
    public GenerateService(IInformationService informationService)
    {
        _informationService = informationService;
    }

    public async Task GenerateCrudFiles(GenerateFilter generateFilter)
    {
        // Geração de arquivos backend
        await GenerateFileAsync("Controller", generateFilter, "Api\\Controllers", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
        await GenerateFileAsync("Entity", generateFilter, "Domain\\Entities", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
        await GenerateFileAsync("EntityConfiguration", generateFilter, "Domain\\Mapping", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
        await GenerateFileAsync("PostDTO", generateFilter, "Domain\\DTO\\Request", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
        await GenerateFileAsync("GetDTO", generateFilter, "Domain\\DTO\\Response", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
        await GenerateFileAsync("ServiceInterface", generateFilter, $"Services\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
        await GenerateFileAsync("Service", generateFilter, $"Services\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
        await GenerateFileAsync("Validator", generateFilter, $"Services\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
        await GenerateFileAsync("Mapper", generateFilter, $"Api\\AutoMapper", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);

        if (generateFilter.TableColumnsFilter.Count() > 0)
            await GenerateFileAsync("Filter", generateFilter, "Domain\\Filter", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);

        await ModifyFileWithTemplateAsync(
            filePath: Path.Combine(generateFilter.GenerateBackendFilter.ProjectApiPath, "Api\\AutoMapper\\ConfigureMap.cs"),
            entityName: generateFilter.EntityName,
            insertAfterRegex: @"var\s+mapperConfig\s*=\s*new\s+MapperConfiguration\s*\(\s*cfg\s*=>\s*\{",
            templateText: "cfg.AddProfile<{{ entity_name }}Map>();"
        );

        await ModifyFileWithTemplateAsync(
            filePath: Path.Combine(generateFilter.GenerateBackendFilter.ProjectApiPath, "Api\\Configuration\\DependencyInjectionConfig.cs"),
            entityName: generateFilter.EntityName,
            insertAfter: "services.AddSingleton(configuration);",
            templateText: "services.AddTransient<I{{ entity_name }}Service, {{ entity_name }}Service>();"
        );

        // Geração de arquivos frontend
        await GenerateFileAsync("Service", generateFilter, "src\\app\\services", generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);
    }

    public void ValidateProjectStructure(string projectApiRootPath, string projectClientRootPath)
    {
        if (string.IsNullOrWhiteSpace(projectApiRootPath))
            throw new ValidationException("Caminho da API do projeto não pode ser vazia.");
        if (string.IsNullOrWhiteSpace(projectApiRootPath))
            throw new ValidationException("Caminho do Cliente do projeto não pode ser vazio.");

        var requiredApiPaths = new[]
        {
            "Api\\Configuration\\DependencyInjectionConfig.cs",
            "Api\\AutoMapper\\ConfigureMap.cs",
            "Api\\Controllers",
            "Domain\\Entities",
            "Domain\\Mapping",
            "Domain\\DTO\\Request",
            "Domain\\DTO\\Response",
            "Services"
        };

        var requiredClientPaths = new[]
        {
            "src\\app\\app.module.ts",
            "src\\app\\app.routes.module.ts",
            "src\\app\\services",
            "src\\app\\models",
            "src\\app\\modulos",
        };


        foreach (var relativePath in requiredApiPaths)
        {
            var fullPath = Path.Combine(projectApiRootPath, relativePath);
            EnsurePathExists(fullPath);
        }
        foreach (var relativePath in requiredClientPaths)
        {
            var fullPath = Path.Combine(projectClientRootPath, relativePath);
            EnsurePathExists(fullPath);
        }
    }

    private async Task GenerateFileAsync(string fileType, GenerateFilter filter, string targetDirectory, string projectPath, TemplateType templateType)
    {
        string templateContent = await LoadTemplateContentAsync(fileType + "Template", templateType);

        var infoTable = filter.TableName != null ? await _informationService.GetInformationsByTableName(filter.ConnectionFilter, filter.TableName) : null;

        var tableColumnsFilterList = infoTable?.Where(info => filter.TableColumnsFilter != null && filter.TableColumnsFilter.Contains(info.ColumnName)).ToList();

        var output = RenderTemplate(templateContent, new { filter.EntityName, filter.IsServerSide, tableColumnsFilterList, infoTable });

        string directoryPath = Path.Combine(projectPath, targetDirectory);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string fileName = GetFileName(fileType, filter.EntityName, templateType);
        string outputPath = Path.Combine(directoryPath, fileName);

        await File.WriteAllTextAsync(outputPath, output);
    }

    private async Task ModifyFileWithTemplateAsync(string filePath,
                                                   string entityName,
                                                   string insertAfter = null,
                                                   string insertBefore = null,
                                                   string insertAfterRegex = null,
                                                   string insertBeforeRegex = null,
                                                   string templateText = null,
                                                   bool avoidDuplicates = true)
    {
        if (!File.Exists(filePath))
            throw new ValidationException($"Arquivo não encontrado: {filePath}");

        string fileContent = await File.ReadAllTextAsync(filePath);

        string renderedLine = RenderTemplate(templateText, new { entityName });

        if (avoidDuplicates && fileContent.Contains(renderedLine))
            return;

        string updatedContent = InsertRenderedLine(fileContent, renderedLine, insertAfter, insertBefore, insertAfterRegex, insertBeforeRegex);
        await File.WriteAllTextAsync(filePath, updatedContent);
    }

    private async Task<string> LoadTemplateContentAsync(string templateName, TemplateType templateType)
    {
        string baseTemplateDirectory = templateType switch
        {
            TemplateType.Api => ApiTemplatesDirectory,
            TemplateType.Client => ClientTemplatesDirectory,
            _ => throw new ValidationException("Tipo de template inválido")
        };

        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), baseTemplateDirectory, templateName + ".tlp");

        if (!File.Exists(templatePath))
            throw new ValidationException($"Template não encontrado: {templatePath}");

        return await File.ReadAllTextAsync(templatePath);
    }

    private string RenderTemplate(string content, object model)
    {
        var template = Template.Parse(content);
        return template.Render(model).Trim();
    }

    private string InsertRenderedLine(string content, string renderedLine, string insertAfter, string insertBefore, string insertAfterRegex, string insertBeforeRegex)
    {
        if (!string.IsNullOrEmpty(insertAfterRegex))
        {
            var match = Regex.Match(content, insertAfterRegex);
            if (match.Success)
                return content.Insert(match.Index + match.Length, "\n            " + renderedLine);
            throw new ValidationException($"Regex não encontrou ponto de inserção: {insertAfterRegex}");
        }

        if (!string.IsNullOrEmpty(insertBeforeRegex))
        {
            var match = Regex.Match(content, insertBeforeRegex);
            if (match.Success)
                return content.Insert(match.Index, "            " + renderedLine + "\n");
            throw new ValidationException($"Regex não encontrou ponto de inserção: {insertBeforeRegex}");
        }

        if (!string.IsNullOrEmpty(insertAfter))
        {
            int index = content.IndexOf(insertAfter);
            if (index != -1)
                return content.Insert(index + insertAfter.Length, "\n        " + renderedLine);
        }

        if (!string.IsNullOrEmpty(insertBefore))
        {
            int index = content.IndexOf(insertBefore);
            if (index != -1)
                return content.Insert(index, "            " + renderedLine + "\n");
        }

        throw new ValidationException("É necessário definir um ponto de inserção.");
    }

    private void EnsurePathExists(string fullPath)
    {
        if (File.Exists(fullPath) || Directory.Exists(fullPath)) return;

        if (Path.HasExtension(fullPath))
            throw new ValidationException($"Caminho inválido. Arquivo obrigatório não encontrado: {fullPath}");
        else
            throw new ValidationException($"Caminho inválido. Diretório obrigatório não encontrado: {fullPath}");
    }

    private string GetFileName(string fileType, string entityName, TemplateType templateType)
    {
        return templateType switch
        {
            TemplateType.Api => fileType switch
            {
                "ServiceInterface" => $"I{entityName}Service.cs",
                "Entity" => $"{entityName}.cs",
                "Mapper" => $"{entityName}Map.cs",
                _ => $"{entityName}{fileType}.cs"
            },
            TemplateType.Client => fileType switch
            {
                "Service" => $"{entityName.ToLowerFirst()}.service.ts",
                "Model" => $"{entityName}.ts",
                _ => $"{entityName.ToLowerFirst()}.{fileType.ToLower()}.ts"
            },
            _ => throw new ValidationException("Tipo de template não suportado.")
        };
    }
}
