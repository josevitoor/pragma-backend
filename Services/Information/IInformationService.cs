using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Filter;
using TCE.Base.Services;

namespace Services;

public interface IInformationService : IService<Information>
{
    Task<IEnumerable<Information>> GetInfoByTableName(string tableName);
    Task GenerateCrudFiles(InformationFilter informationFilter);
}