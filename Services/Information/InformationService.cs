using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using FluentValidation;

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
    public async Task<IEnumerable<Information>> BdConnection(ConnectionFilter filter)
    {
        var connectionString = $"Data Source={filter.Host},{filter.Port};uid={filter.User};password={filter.Password};Initial Catalog={filter.Database};";

        var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var dbContext = new DynamicDbContext(optionsBuilder.Options);


        var canConnect = await dbContext.Database.CanConnectAsync();

        if (!canConnect)
            throw new ValidationException("Não foi possível conectar ao banco de dados com os dados fornecidos.");

        return await dbContext.Informations.ToListAsync();
    }
}
