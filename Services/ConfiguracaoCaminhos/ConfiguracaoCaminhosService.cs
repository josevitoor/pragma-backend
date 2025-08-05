using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;
using System.Threading.Tasks;
using System.Collections.Generic;
using TCE.Base.Token;

namespace Services;

public class ConfiguracaoCaminhosService : BaseService<ConfiguracaoCaminhos>, IConfiguracaoCaminhosService
{
    public ConfiguracaoCaminhosService(IUnitOfWork uow) : base(uow)
    {
    }

    public async Task<IEnumerable<ConfiguracaoCaminhos>> GetAllByOperadorAsync()
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        return await GetAllAsync(predicate: x => x.IdOperadorInclusao == int.Parse(tokenInfo.IdOperador));
    }

    public async Task<ConfiguracaoCaminhos> GetByIdAsync(int id)
    {
        ConfiguracaoCaminhos configuracaoCaminhos = await base.GetByIdAsync(predicate: item => item.IdConfiguracaoCaminho == id);

        return configuracaoCaminhos;
    }

    public ConfiguracaoCaminhos Add(ConfiguracaoCaminhos configuracaoCaminhos)
    {
        return base.Add<ConfiguracaoCaminhosValidator>(configuracaoCaminhos);
    }

    public ConfiguracaoCaminhos Update(ConfiguracaoCaminhos configuracaoCaminhos)
    {
        return base.Update<ConfiguracaoCaminhosValidator>(configuracaoCaminhos);
    }
}