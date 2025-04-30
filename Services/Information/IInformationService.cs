using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using TCE.Base.Services;

namespace Services;

public interface IInformationService : IService<Information>
{
    Task<IEnumerable<Information>> GetInfoByTableName(string tableName);
    Task<IEnumerable<string>> GetAllTableSelect();
}