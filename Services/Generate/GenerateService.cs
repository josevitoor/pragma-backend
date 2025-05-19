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

    public async Task GenerateBackendCrudFiles(GenerateBackendFilter generateBackendFilter)
    {
        await GenerateFileAsync("Controller", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, "Api\\Controllers");
        await GenerateFileAsync("Entity", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, "Domain\\Entities", generateBackendFilter.TableName);
        await GenerateFileAsync("EntityConfiguration", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, "Domain\\Mapping", generateBackendFilter.TableName);
        await GenerateFileAsync("PostDTO", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, "Domain\\DTO\\Request", generateBackendFilter.TableName);
        await GenerateFileAsync("GetDTO", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, "Domain\\DTO\\Response", generateBackendFilter.TableName);
        await GenerateFileAsync("ServiceInterface", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, $"Services\\{generateBackendFilter.EntityName}", generateBackendFilter.TableName);
        await GenerateFileAsync("Service", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, $"Services\\{generateBackendFilter.EntityName}", generateBackendFilter.TableName);
        await GenerateFileAsync("Validator", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, $"Services\\{generateBackendFilter.EntityName}", generateBackendFilter.TableName);
        await GenerateFileAsync("Mapper", generateBackendFilter.EntityName, generateBackendFilter.ProjectApiPath, $"Api\\AutoMapper", generateBackendFilter.TableName);

        await ModifyFileWithTemplateAsync(
            filePath: Path.Combine(generateBackendFilter.ProjectApiPath, "Api\\AutoMapper\\ConfigureMap.cs"),
            entityName: generateBackendFilter.EntityName,
            insertAfterRegex: @"var\s+mapperConfig\s*=\s*new\s+MapperConfiguration\s*\(\s*cfg\s*=>\s*\{",
            templateText: "cfg.AddProfile<{{ entity_name }}Map>();"
        );

        await ModifyFileWithTemplateAsync(
            filePath: Path.Combine(generateBackendFilter.ProjectApiPath, "Api\\Configuration\\DependencyInjectionConfig.cs"),
            entityName: generateBackendFilter.EntityName,
            insertAfter: "services.AddSingleton(configuration);",
            templateText: "services.AddTransient<I{{ entity_name }}Service, {{ entity_name }}Service>();"
        );
    }

    public void ValidateProjectStructure(string projectRootPath)
    {
        if (string.IsNullOrWhiteSpace(projectRootPath))
            throw new ValidationException("Caminho do projeto não pode ser vazio.");

        var requiredPaths = new[]
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

        foreach (var relativePath in requiredPaths)
        {
            var fullPath = Path.Combine(projectRootPath, relativePath);
            EnsurePathExists(fullPath);
        }
    }

    private async Task GenerateFileAsync(string fileType, string entityName, string projectApiPath, string targetDirectory, string tableName = null)
    {
        string templateContent = await LoadTemplateContentAsync(fileType + "Template");

        var infoTable = tableName != null ? await _informationService.GetInfoByTableName(tableName) : null;
        var output = RenderTemplate(templateContent, new { entityName, infoTable });

        string directoryPath = Path.Combine(projectApiPath, targetDirectory);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string fileName = fileType switch
        {
            "ServiceInterface" => "I" + entityName + "Service.cs",
            "Entity" => entityName + ".cs",
            "Mapper" => entityName + "Map.cs",
            _ => entityName + fileType + ".cs"
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
