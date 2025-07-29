using Domain.Entities;
using TCE.Base.Services;

namespace Services;

public interface IConfiguracaoGeracaoService : IService<ConfiguracaoGeracao>
{
    public ConfiguracaoGeracao Add(ConfiguracaoGeracao configuracaoGeracao);
    public ConfiguracaoGeracao Update(ConfiguracaoGeracao configuracaoGeracao);
}