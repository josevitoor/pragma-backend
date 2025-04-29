using System.Threading.Tasks;
using Domain.Filter;

namespace Services;

public interface IGenerateService
{
    Task GenerateCrudFiles(InformationFilter informationFilter);
}