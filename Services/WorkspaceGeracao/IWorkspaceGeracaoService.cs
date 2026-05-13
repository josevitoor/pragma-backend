using Domain.Entities;
using TCE.Base.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services;
public interface IWorkspaceGeracaoService : IService<WorkspaceGeracao>
{
    public Task<IEnumerable<WorkspaceGeracao>> GetAllAsync();
    public Task<IEnumerable<WorkspaceGeracao>> GetAllByOperador();
    public Task<WorkspaceGeracao> GetByIdAsync(int id);
    public WorkspaceGeracao Add(WorkspaceGeracao workspaceGeracao);
    public WorkspaceGeracao Update(WorkspaceGeracao workspaceGeracao);
}