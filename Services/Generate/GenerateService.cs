using System.Threading.Tasks;
using Scriban;
using System.IO;
using Domain.Filter;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Services;

public class GenerateService : IGenerateService
{
    private const string TemplatesDirectory = "Templates\\Automation";
    private readonly IInformationService _informationService;
    public GenerateService(IInformationService informationService)
    {
        _informationService = informationService;
    }

    public async Task GenerateCrudFiles(GenerateFilter generateFilter)
    {
        await GenerateFileAsync("Controller", generateFilter, "Api\\Controllers", generateFilter.GenerateBackendFilter.ProjectApiPath);
        await GenerateFileAsync("Entity", generateFilter, "Domain\\Entities", generateFilter.GenerateBackendFilter.ProjectApiPath);
        await GenerateFileAsync("EntityConfiguration", generateFilter, "Domain\\Mapping", generateFilter.GenerateBackendFilter.ProjectApiPath);
        await GenerateFileAsync("PostDTO", generateFilter, "Domain\\DTO\\Request", generateFilter.GenerateBackendFilter.ProjectApiPath);
        await GenerateFileAsync("GetDTO", generateFilter, "Domain\\DTO\\Response", generateFilter.GenerateBackendFilter.ProjectApiPath);
        await GenerateFileAsync("ServiceInterface", generateFilter, $"Services\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath);
        await GenerateFileAsync("Service", generateFilter, $"Services\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath);
        await GenerateFileAsync("Validator", generateFilter, $"Services\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath);
        await GenerateFileAsync("Mapper", generateFilter, $"Api\\AutoMapper", generateFilter.GenerateBackendFilter.ProjectApiPath);

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

        foreach (var relativePath in requiredApiPaths)
        {
            var fullPath = Path.Combine(projectApiRootPath, relativePath);
            EnsurePathExists(fullPath);
        }
    }

    private async Task GenerateFileAsync(string fileType, GenerateFilter filter, string targetDirectory, string projectPath)
    {
        string templateContent = await LoadTemplateContentAsync(fileType + "Template");

        var infoTable = filter.TableName != null ? await _informationService.GetInformationsByTableName(filter.ConnectionFilter, filter.TableName) : null;
        var output = RenderTemplate(templateContent, new { filter.EntityName, infoTable });

        string directoryPath = Path.Combine(projectPath, targetDirectory);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string fileName = fileType switch
        {
            "ServiceInterface" => "I" + filter.EntityName + "Service.cs",
            "Entity" => filter.EntityName + ".cs",
            "Mapper" => filter.EntityName + "Map.cs",
            _ => filter.EntityName + fileType + ".cs"
        };

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

    private async Task<string> LoadTemplateContentAsync(string templateName)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), TemplatesDirectory, templateName + ".tlp");

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
}
