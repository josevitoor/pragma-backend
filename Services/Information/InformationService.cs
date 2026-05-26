using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using FluentValidation;
using Domain;
using Domain.Filter;
using Domain.DTO.Request;
using System.Data.SqlClient;
using System;

namespace Services;

public class InformationService : BaseService<Information>, IInformationService
{
    private readonly IUnitOfWork _uow;
    public InformationService(IUnitOfWork uow) : base(uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Information>> GetInformationsByTableName(ConnectionFilter filter, string tableName)
    {
        using var dbContext = await CreateDynamicDbContext(filter);

        return await dbContext.Informations
            .Where(i => i.TableName == tableName)
            .Include(i => i.TableConstraint)
                .ThenInclude(tc => tc.ConstraintInfo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Information>> GetAllInformations(ConnectionFilter filter)
    {
        using var dbContext = await CreateDynamicDbContext(filter);

        return await dbContext.Informations
            .Include(i => i.TableConstraint)
                .ThenInclude(tc => tc.ConstraintInfo)
            .ToListAsync();
    }

    private async static Task<DynamicDbContext> CreateDynamicDbContext(ConnectionFilter filter)
    {
        var connectionString = $"Data Source={filter.Servidor},{filter.Porta};uid={filter.Usuario};password={filter.Senha};Initial Catalog={filter.BaseDados};";

        var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var dbContext = new DynamicDbContext(optionsBuilder.Options);

        var canConnect = await dbContext.Database.CanConnectAsync();

        if (!canConnect)
            throw new ValidationException("Não foi possível conectar ao banco de dados com os dados fornecidos.");

        return dbContext;
    }

    public async Task ExecuteScript(ExecuteScriptDTO dto)
    {
        using var dbContext = await CreateDynamicDbContext(dto.Filter);
       
        using var transaction = dbContext.Database.BeginTransaction();
        try
        {
            foreach (var tabela in dto.Tabelas)
            {
                var exists = await dbContext.Informations.AnyAsync(i => i.TableName.ToLower() == tabela.ToLower());
                if (exists)
                {
                    throw new ValidationException($"A tabela '{tabela}' já existe no banco, escolha outro tipo de geração de código.");
                }
            }

            await dbContext.Database.ExecuteSqlRawAsync(dto.Script);
            transaction.Commit();
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw new ValidationException($"Erro ao executar o script SQL: {ex.Message}");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            var message = ex.InnerException?.Message ?? ex.Message;
            throw new ValidationException($"Erro ao executar o script SQL: {message}");
        }
    }
}
