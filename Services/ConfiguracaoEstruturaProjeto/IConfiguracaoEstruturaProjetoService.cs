using Domain.Entities;
using TCE.Base.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services;

public interface IConfiguracaoEstruturaProjetoService : IService<ConfiguracaoEstruturaProjeto>
{
    public Task<IEnumerable<ConfiguracaoEstruturaProjeto>> GetAllAsync();
    public Task<ConfiguracaoEstruturaProjeto> GetByIdAsync(int id);
    public ConfiguracaoEstruturaProjeto Add(ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto);
    public ConfiguracaoEstruturaProjeto Update(ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto);
    public Task DeleteAsync(ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto);
}