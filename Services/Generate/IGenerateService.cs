using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Filter;

namespace Services;

public interface IGenerateService
{
    Task GenerateCrudFilesList(GenerateBatchRequest generateBatchRequest);
}