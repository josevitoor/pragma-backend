using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Filter;
using TCE.Base.Services;

namespace Services;

public interface IInformationService : IService<Information>
{
    Task<IEnumerable<Information>> GetInformationsByTableName(ConnectionFilter filter, string tableName);
    Task<IEnumerable<Information>> GetAllInformations(ConnectionFilter filter);
}