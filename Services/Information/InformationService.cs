using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using Scriban;
using System.IO;
using Domain.Filter;

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

        string fileName = fileType == "Entity" ? entityName + ".cs" : entityName + fileType + ".cs";
        string outputPath = Path.Combine(directoryPath, fileName);

        await File.WriteAllTextAsync(outputPath, output);
    }
}
