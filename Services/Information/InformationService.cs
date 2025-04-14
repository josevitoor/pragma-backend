using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using Scriban;
using System.IO;
using Domain.Filter;
using System;
using System.Text.RegularExpressions;

namespace Services;
public class InformationService : BaseService<Information>, IInformationService
{
    private const string TemplatesDirectory = "Templates/Automation";
    public InformationService(IUnitOfWork uow) : base(uow)
    {
    }

    public async Task<IEnumerable<Information>> GetInfoByTableName(string tableName)
    {
        return await GetAllAsync(x => x.TableName == tableName);
    }

    public async Task GenerateCrudFiles(InformationFilter informationFilter)
    {
        await GenerateFileAsync("Controller", informationFilter.EntityName, informationFilter.ProjectPath, "Api/Controllers");
        await GenerateFileAsync("Entity", informationFilter.EntityName, informationFilter.ProjectPath, "Domain/Entities", informationFilter.TableName);
        await GenerateFileAsync("EntityConfiguration", informationFilter.EntityName, informationFilter.ProjectPath, "Domain/Mapping", informationFilter.TableName);
        await GenerateFileAsync("PostDTO", informationFilter.EntityName, informationFilter.ProjectPath, "Domain/DTO/Request", informationFilter.TableName);
        await GenerateFileAsync("GetDTO", informationFilter.EntityName, informationFilter.ProjectPath, "Domain/DTO/Response", informationFilter.TableName);
        await GenerateFileAsync("ServiceInterface", informationFilter.EntityName, informationFilter.ProjectPath, $"Services/{informationFilter.EntityName}", informationFilter.TableName);
        await GenerateFileAsync("Service", informationFilter.EntityName, informationFilter.ProjectPath, $"Services/{informationFilter.EntityName}", informationFilter.TableName);
        await GenerateFileAsync("Validator", informationFilter.EntityName, informationFilter.ProjectPath, $"Services/{informationFilter.EntityName}", informationFilter.TableName);
        await GenerateFileAsync("Mapper", informationFilter.EntityName, informationFilter.ProjectPath, $"Api/AutoMapper", informationFilter.TableName);
        await ModifyFileWithTemplateAsync(filePath: Path.Combine(informationFilter.ProjectPath, "Api/AutoMapper/ConfigureMap.cs"),
                                            templateModel: new { informationFilter.EntityName },
                                            insertAfterRegex: @"var\s+mapperConfig\s*=\s*new\s+MapperConfiguration\s*\(\s*cfg\s*=>\s*\{",
                                            templateText: "cfg.AddProfile<{{ entity_name }}Map>();");
        await ModifyFileWithTemplateAsync(filePath: Path.Combine(informationFilter.ProjectPath, "Api/Configuration/DependencyInjectionConfig.cs"),
                                            templateModel: new { informationFilter.EntityName },
                                            insertAfter: "services.AddSingleton(configuration);",
                                            templateText: "services.AddTransient<I{{ entity_name }}Service, {{ entity_name }}Service>();");
    }

    private async Task GenerateFileAsync(string fileType, string entityName, string projectPath, string targetDirectory, string tableName = null)
    {

        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), TemplatesDirectory, fileType + "Template.tlp");

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Template não encontrado: {templatePath}");

        var templateContent = File.ReadAllText(templatePath);
        var template = Template.Parse(templateContent);

        var infoTable = tableName != null ? await GetInfoByTableName(tableName) : null;

        var output = template.Render(new { entityName, infoTable });

        string directoryPath = Path.Combine(projectPath, targetDirectory);
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
                                                    object templateModel,
                                                    string insertAfter = null,
                                                    string insertBefore = null,
                                                    string insertAfterRegex = null,
                                                    string insertBeforeRegex = null,
                                                    string templateText = null,
                                                    string templateFileName = null,
                                                    bool avoidDuplicates = true)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

        string fileContent = await File.ReadAllTextAsync(filePath);

        string templateContent;

        if (!string.IsNullOrEmpty(templateText))
        {
            templateContent = templateText;
        }
        else if (!string.IsNullOrEmpty(templateFileName))
        {
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), TemplatesDirectory, templateFileName + ".tlp");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template não encontrado: {templatePath}");

            templateContent = await File.ReadAllTextAsync(templatePath);
        }
        else
        {
            throw new ArgumentException("Você deve fornecer 'templateText' ou 'templateFileName'.");
        }

        var template = Template.Parse(templateContent);
        var renderedLine = template.Render(templateModel).Trim();

        if (avoidDuplicates && fileContent.Contains(renderedLine))
            return;

        string updatedContent = fileContent;

        if (!string.IsNullOrEmpty(insertAfterRegex))
        {
            var match = Regex.Match(fileContent, insertAfterRegex);
            if (match.Success)
            {
                int insertPos = match.Index + match.Length;
                updatedContent = fileContent.Insert(insertPos, "\n            " + renderedLine);
            }
            else
            {
                throw new InvalidOperationException($"Regex não encontrou ponto de inserção: {insertAfterRegex}");
            }
        }
        else if (!string.IsNullOrEmpty(insertBeforeRegex))
        {
            var match = Regex.Match(fileContent, insertBeforeRegex);
            if (match.Success)
            {
                updatedContent = fileContent.Insert(match.Index, "            " + renderedLine + "\n");
            }
            else
            {
                throw new InvalidOperationException($"Regex não encontrou ponto de inserção: {insertBeforeRegex}");
            }
        }
        else if (!string.IsNullOrEmpty(insertAfter))
        {
            int index = fileContent.IndexOf(insertAfter);
            if (index != -1)
            {
                int insertPos = index + insertAfter.Length;
                updatedContent = fileContent.Insert(insertPos, "\n        " + renderedLine);
            }
        }
        else if (!string.IsNullOrEmpty(insertBefore))
        {
            int index = fileContent.IndexOf(insertBefore);
            if (index != -1)
            {
                updatedContent = fileContent.Insert(index, "            " + renderedLine + "\n");
            }
        }
        else
        {
            throw new InvalidOperationException("É necessário definir um ponto de inserção.");
        }

        await File.WriteAllTextAsync(filePath, updatedContent);
    }
}
