using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using Scriban;
using System.IO;
using Domain.Filter;
using System;

namespace Services;
public class InformationService : BaseService<Information>, IInformationService
{
    public InformationService(IUnitOfWork uow) : base(uow)
    {
    }

    public async Task<IEnumerable<Information>> GetInfoByTableName(string tableName)
    {
        return await GetAllAsync(x => x.TableName == tableName);
    }

    public async Task GenerateCrudFiles(InformationFilter informationFilter)
    {
        await GenerateControllerFile(informationFilter.EntityName, informationFilter.ProjectPath);
    }

    private async Task GenerateControllerFile(string entityName, string projectPath)
    {
        string controllerTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Automation", "ControllerTemplate.tlp");

        if (!File.Exists(controllerTemplatePath))
            throw new FileNotFoundException($"Template não encontrado: {controllerTemplatePath}");

        var controllerTemplateContent = File.ReadAllText(controllerTemplatePath);
        var controllerTemplate = Template.Parse(controllerTemplateContent);
        string varEntityName = Char.ToLowerInvariant(entityName[0]) + entityName.Substring(1);
        var controllerOutput = controllerTemplate.Render(new { entityName, varEntityName });

        string controllersDirectory = Path.Combine(projectPath, "Api", "Controllers", entityName);

        if (!Directory.Exists(controllersDirectory))
            Directory.CreateDirectory(controllersDirectory);

        string outputPath = Path.Combine(controllersDirectory, $"{entityName}Controller.cs");
        await File.WriteAllTextAsync(outputPath, controllerOutput);
    }
}
