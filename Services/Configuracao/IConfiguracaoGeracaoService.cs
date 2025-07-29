using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using TCE.Base.Services;

namespace Services;

public interface IConfiguracaoGeracaoService : IService<ConfiguracaoGeracao>
{
    public Task<IEnumerable<ConfiguracaoGeracao>> GetAllByOperador();
    public ConfiguracaoGeracao Add(ConfiguracaoGeracao configuracaoGeracao);
    public ConfiguracaoGeracao Update(ConfiguracaoGeracao configuracaoGeracao);
}