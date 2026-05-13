using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Domain.Enum;

namespace Services;
public class WorkspaceGeracaoService : BaseService<WorkspaceGeracao>, IWorkspaceGeracaoService
{
    public WorkspaceGeracaoService(IUnitOfWork uow) : base(uow)
    {
    }

    public async Task<IEnumerable<WorkspaceGeracao>> GetAllAsync()
    {
        return await base.GetAllAsync();
    }

    public async Task<IEnumerable<WorkspaceGeracao>> GetAllByOperador()
    {
        var idOperador = int.Parse(_sessao.IdOperador);
        return await GetAllAsync(predicate: x => x.IdOperadorInclusao == idOperador);
    }

    public async Task<WorkspaceGeracao> GetByIdAsync(int id)
    {
        WorkspaceGeracao workspaceGeracao = await base.GetByIdAsync(predicate: item => item.IdWorkspaceGeracao == id);

        return workspaceGeracao;
    }

    public WorkspaceGeracao Add(WorkspaceGeracao workspaceGeracao)
    {
        workspaceGeracao.DataInclusao = DateTime.Now;
        workspaceGeracao.IdOperadorInclusao = int.Parse(_sessao.IdOperador);

        return base.Add<WorkspaceGeracaoValidator>(workspaceGeracao);
    }

    public WorkspaceGeracao Update(WorkspaceGeracao workspaceGeracao)
    {
        return base.Update<WorkspaceGeracaoValidator>(workspaceGeracao);
    }
}