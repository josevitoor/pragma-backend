using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using System.Threading.Tasks;
using System.Collections.Generic;
using TCE.Base.Token;
using FluentValidation;

namespace Services;

public class ConfiguracaoEstruturaProjetoService : BaseService<ConfiguracaoEstruturaProjeto>, IConfiguracaoEstruturaProjetoService
{
    public ConfiguracaoEstruturaProjetoService(IUnitOfWork uow) : base(uow)
    {
    }

    public async Task<IEnumerable<ConfiguracaoEstruturaProjeto>> GetAllAsync()
    {
        return await base.GetAllAsync();
    }

    public async Task<ConfiguracaoEstruturaProjeto> GetByIdAsync(int id)
    {
        ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto = await base.GetByIdAsync(predicate: item => item.IdConfiguracaoEstrutura == id);

        return configuracaoEstruturaProjeto;
    }

    public ConfiguracaoEstruturaProjeto Add(ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto)
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        configuracaoEstruturaProjeto.IdOperadorInclusao = int.Parse(tokenInfo.IdOperador);
        return base.Add<ConfiguracaoEstruturaProjetoValidator>(configuracaoEstruturaProjeto);
    }

    public ConfiguracaoEstruturaProjeto Update(ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto)
    {
        return base.Update<ConfiguracaoEstruturaProjetoValidator>(configuracaoEstruturaProjeto);
    }

    public async Task DeleteAsync(ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto)
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        if (configuracaoEstruturaProjeto.IdOperadorInclusao != int.Parse(tokenInfo.IdOperador))
        {
            throw new ValidationException("Somente o operador responsável pela inclusão desta configuração pode excluí-la.");
        }

        Delete(configuracaoEstruturaProjeto);
    }
}