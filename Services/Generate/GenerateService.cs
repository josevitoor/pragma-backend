using System.Threading.Tasks;
using Scriban;
using System.IO;
using Domain.Filter;
using System.Text.RegularExpressions;
using FluentValidation;
using System.Linq;
using Domain.Enum;
using CrossCutting.Util;
using System.Text.Json;
using System;
using LinqKit;
using Domain.Entities;

namespace Services;

public class GenerateService : IGenerateService
{
    private const string ApiTemplatesDirectory = "Templates\\Pragma\\BackendTemplates";
    private const string ClientTemplatesDirectory = "Templates\\Pragma\\FrontendTemplates";
    private readonly IInformationService _informationService;
    private readonly IConfiguracaoEstruturaProjetoService _configEstruturaService;
    public GenerateService(IInformationService informationService, IConfiguracaoEstruturaProjetoService configEstruturaService)
    {
        _informationService = informationService;
        _configEstruturaService = configEstruturaService;
    }

    public async Task GenerateCrudFiles(GenerateFilter generateFilter)
    {
        try
        {
            ConfiguracaoEstruturaProjeto estrutura = await _configEstruturaService.GetByIdAsync(predicate: x => x.IdConfiguracaoEstrutura == generateFilter.IdConfiguracaoEstrutura);

            // Geração de arquivos backend
            await GenerateFileAsync("Controller", generateFilter, estrutura.ApiControllers, generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api, estrutura);
            await GenerateFileAsync("Entity", generateFilter, estrutura.ApiEntities, generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
            await GenerateFileAsync("EntityConfiguration", generateFilter, estrutura.ApiMapping, generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
            await GenerateFileAsync("PostDTO", generateFilter, "Domain\\DTO\\Request", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
            await GenerateFileAsync("GetDTO", generateFilter, "Domain\\DTO\\Response", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
            await GenerateFileAsync("ServiceInterface", generateFilter, $"{estrutura.ApiServices}\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api, estrutura);
            await GenerateFileAsync("Service", generateFilter, $"{estrutura.ApiServices}\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api, estrutura);
            await GenerateFileAsync("Validator", generateFilter, $"{estrutura.ApiServices}\\{generateFilter.EntityName}", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);
            await GenerateFileAsync("Mapper", generateFilter, $"Api\\AutoMapper", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api, estrutura);

            if (generateFilter.TableColumnsFilter.Any())
                await GenerateFileAsync("Filter", generateFilter, "Domain\\Filter", generateFilter.GenerateBackendFilter.ProjectApiPath, TemplateType.Api);

            await ModifyFileWithTemplateAsync(
                filePath: Path.Combine(generateFilter.GenerateBackendFilter.ProjectApiPath, estrutura.ApiConfigureMap),
                entityName: generateFilter.EntityName,
                insertAfterRegex: @"cfg\.AddProfile<.*?>\s*\(\);",
                templateText: "cfg.AddProfile<{{ entity_name }}Map>();"
            );

            await ModifyFileWithTemplateAsync(
                filePath: Path.Combine(generateFilter.GenerateBackendFilter.ProjectApiPath, estrutura.ApiDependencyInjectionConfig),
                entityName: generateFilter.EntityName,
                insertAfterRegex: @"services\.AddSingleton(?:<IConfiguration>)?\(([cC]onfiguration)\);",
                templateText: "services.AddTransient<I{{ entity_name }}Service, {{ entity_name }}Service>();"
            );

            await ModifyFileWithTemplateAsync(
                filePath: Path.Combine(generateFilter.GenerateBackendFilter.ProjectApiPath, estrutura.ApiContexts),
                entityName: generateFilter.EntityName,
                insertAfterRegex: @"modelBuilder\.ApplyConfiguration\(.*\);",
                templateText: "modelBuilder.ApplyConfiguration(new {{ entity_name }}EntityConfiguration());"
            );

            // Geração de arquivos frontend
            await GenerateFileAsync("Service", generateFilter, estrutura.ClientServices, generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);
            await GenerateFileAsync("Model", generateFilter, estrutura.ClientModels, generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);
            await GenerateFileAsync("Module", generateFilter, $"{estrutura.ClientModulos}\\{generateFilter.EntityName.ToLowerFirst()}", generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);
            await GenerateFileAsync("Routing", generateFilter, $"{estrutura.ClientModulos}\\{generateFilter.EntityName.ToLowerFirst()}", generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);
            await GenerateFileAsync("ListHtml", generateFilter, $"{estrutura.ClientModulos}\\{generateFilter.EntityName.ToLowerFirst()}\\{generateFilter.EntityName.ToKebabCase()}-list", generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);
            await GenerateFileAsync("ListTs", generateFilter, $"{estrutura.ClientModulos}\\{generateFilter.EntityName.ToLowerFirst()}\\{generateFilter.EntityName.ToKebabCase()}-list", generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);
            await GenerateFileAsync("FormHtml", generateFilter, $"{estrutura.ClientModulos}\\{generateFilter.EntityName.ToLowerFirst()}\\{generateFilter.EntityName.ToKebabCase()}-form", generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);
            await GenerateFileAsync("FormTs", generateFilter, $"{estrutura.ClientModulos}\\{generateFilter.EntityName.ToLowerFirst()}\\{generateFilter.EntityName.ToKebabCase()}-form", generateFilter.GenerateFrontendFilter.ProjectClientPath, TemplateType.Client);

            var filePathRotas = Path.Combine(generateFilter.GenerateFrontendFilter.ProjectClientPath, estrutura.ClientArquivoRotas);

            string clientModulos = estrutura.ClientModulos.Replace("\\", "/");
            string templateText =
                "{ path: '{{ kebab_case }}', loadChildren: () => import('"
                + clientModulos
                + "/{{ entity_name | string.slice(0, 1) | string.downcase }}{{ entity_name | string.slice(1) }}/{{ kebab_case }}.module').then(m => m.{{ entity_name }}Module) },";

            var content = await File.ReadAllTextAsync(filePathRotas);

            if (Regex.IsMatch(content, @"{ path:\s*['""]404['""]"))
            {
                await ModifyFileWithTemplateAsync(
                    filePath: filePathRotas,
                    entityName: generateFilter.EntityName,
                    insertBeforeRegex: @"{ path:\s*['""]404['""]",
                    templateText: templateText
                );
            }
            else if (Regex.IsMatch(content, @"{ path:\s*'\*\*'"))
            {
                await ModifyFileWithTemplateAsync(
                    filePath: filePathRotas,
                    entityName: generateFilter.EntityName,
                    insertBeforeRegex: @"{ path:\s*'\*\*'",
                    templateText: templateText
                );
            }
            else
            {
                await ModifyFileWithTemplateAsync(
                    filePath: filePathRotas,
                    entityName: generateFilter.EntityName,
                    insertAfterRegex: @"children:\s*\[",
                    templateText: templateText
                );
            }
        }
        catch (Exception ex)
        {
            throw new ValidationException("Error ao gerar os arquivos. " + ex);
        }
    }

    private async Task GenerateFileAsync(string fileType, GenerateFilter filter, string targetDirectory, string projectPath, TemplateType templateType, ConfiguracaoEstruturaProjeto estruturaProjeto = null)
    {
        string templateContent = await LoadTemplateContentAsync(fileType + "Template", templateType);

        var infoTable = filter.TableName != null ? await _informationService.GetInformationsByTableName(filter.ConnectionFilter, filter.TableName) : null;

        var primaryKeyColumn = infoTable
            ?.FirstOrDefault(x => x.TableConstraint != null
                                && x.TableConstraint.ConstraintInfo != null
                                && x.TableConstraint.ConstraintInfo.ConstraintType == "PRIMARY KEY")
            ?.ColumnName;

        var tableColumnsFilterList = infoTable?.Where(info => filter.TableColumnsFilter != null && filter.TableColumnsFilter.Contains(info.ColumnName)).ToList();

        tableColumnsFilterList?.ForEach(info =>
        {
            info.ColumnMap = filter.GenerateFrontendFilter.TableColumnsList
                ?.FirstOrDefault(c => c.DatabaseColumn == info.ColumnName);
        });

        filter.GenerateFrontendFilter.TableColumnsList.ForEach(columns =>
        {
            columns.DataType = infoTable?.FirstOrDefault(c => c.ColumnName == columns.DatabaseColumn).DataType;
        });

        var kebabCase = filter.EntityName.ToKebabCase();
        var entityLabel = filter.EntityName.ToLabel();
        var prefix = GetSelector(filter.GenerateFrontendFilter.ProjectClientPath);

        var output = RenderTemplate(templateContent,
            new
            {
                filter.EntityName,
                filter.IsServerSide,
                filter.HasTceBase,
                filter.GenerateFrontendFilter.TableColumnsList,
                tableColumnsFilterList,
                infoTable,
                kebabCase,
                entityLabel,
                prefix,
                primaryKeyColumn,
                importService = estruturaProjeto?.ApiImportInfraService,
                importInterfaceService = estruturaProjeto?.ApiImportInfraIService,
                importUOW = estruturaProjeto?.ApiImportUOW,
                importPaginate = estruturaProjeto?.ApiImportPaginate,
                importPagConverter = estruturaProjeto?.ApiImportPaginateConverter
            });

        string directoryPath = Path.Combine(projectPath, targetDirectory);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string fileName = GetFileName(fileType, filter.EntityName, templateType);
        string outputPath = Path.Combine(directoryPath, fileName);

        await File.WriteAllTextAsync(outputPath, output);
    }

    private static async Task ModifyFileWithTemplateAsync(string filePath,
                                                   string entityName,
                                                   string insertAfter = null,
                                                   string insertBefore = null,
                                                   string insertAfterRegex = null,
                                                   string insertBeforeRegex = null,
                                                   string templateText = null)
    {
        if (!File.Exists(filePath))
            throw new ValidationException($"Arquivo não encontrado: {filePath}");

        string fileContent = await File.ReadAllTextAsync(filePath);

        var kebabCase = entityName.ToKebabCase();

        string renderedLine = RenderTemplate(templateText, new { entityName, kebabCase });

        string updatedContent = InsertRenderedLine(fileContent, renderedLine, insertAfter, insertBefore, insertAfterRegex, insertBeforeRegex);
        await File.WriteAllTextAsync(filePath, updatedContent);
    }

    private static async Task<string> LoadTemplateContentAsync(string templateName, TemplateType templateType)
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

    private static string RenderTemplate(string content, object model)
    {
        var template = Template.Parse(content);
        return template.Render(model).Trim();
    }

    private static string InsertRenderedLine(string content, string renderedLine, string insertAfter, string insertBefore, string insertAfterRegex, string insertBeforeRegex)
    {
        if (!string.IsNullOrEmpty(insertAfterRegex))
        {
            var match = Regex.Match(content, insertAfterRegex);
            if (match.Success)
            {
                string indentation = GetIndentationAt(content, match.Index);
                return content.Insert(match.Index + match.Length, $"{Environment.NewLine}{indentation}{renderedLine}");
            }
            throw new ValidationException($"Regex não encontrou ponto de inserção: {insertAfterRegex}");
        }

        if (!string.IsNullOrEmpty(insertBeforeRegex))
        {
            var match = Regex.Match(content, insertBeforeRegex);
            if (match.Success)
            {
                string indentation = GetIndentationAt(content, match.Index);
                return content.Insert(match.Index, $"{renderedLine}{Environment.NewLine}{indentation}");
            }
            throw new ValidationException($"Regex não encontrou ponto de inserção: {insertBeforeRegex}");
        }

        if (!string.IsNullOrEmpty(insertAfter))
        {
            int index = content.IndexOf(insertAfter);
            if (index != -1)
            {
                string indentation = GetIndentationAt(content, index);
                return content.Insert(index + insertAfter.Length, $"{Environment.NewLine}{indentation}{renderedLine}");
            }
        }

        if (!string.IsNullOrEmpty(insertBefore))
        {
            int index = content.IndexOf(insertBefore);
            if (index != -1)
            {
                string indentation = GetIndentationAt(content, index);
                return content.Insert(index, $"{renderedLine}{Environment.NewLine}{indentation}");
            }
        }

        throw new ValidationException("É necessário definir um ponto de inserção.");
    }

    private static string GetIndentationAt(string content, int index)
    {
        int lineStart = content.LastIndexOf('\n', index);
        if (lineStart == -1) lineStart = 0;
        else lineStart += 1;

        int i = lineStart;
        while (i < content.Length && (content[i] == ' ' || content[i] == '\t'))
            i++;

        return content.Substring(lineStart, i - lineStart);
    }

    private static string GetFileName(string fileType, string entityName, TemplateType templateType)
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
                "Service" => $"{entityName.ToKebabCase()}.service.ts",
                "Model" => $"{entityName}Type.ts",
                "Routing" => $"{entityName.ToKebabCase()}-routing.module.ts",
                "ListHtml" => $"{entityName.ToKebabCase()}-list.component.html",
                "ListTs" => $"{entityName.ToKebabCase()}-list.component.ts",
                "FormHtml" => $"{entityName.ToKebabCase()}-form.component.html",
                "FormTs" => $"{entityName.ToKebabCase()}-form.component.ts",
                _ => $"{entityName.ToKebabCase()}.{fileType.ToLower()}.ts"
            },
            _ => throw new ValidationException("Tipo de template não suportado.")
        };
    }

    private static string GetSelector(string clientProjectPath)
    {
        var angularJsonPath = Path.Combine(clientProjectPath, "angular.json");
        var json = File.ReadAllText(angularJsonPath);
        var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        var projects = root.GetProperty("projects");

        var firstProject = projects.EnumerateObject().First();
        var prefix = firstProject.Value.GetProperty("prefix").GetString();

        return prefix;
    }
}
