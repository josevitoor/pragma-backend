using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using TCE.Base.Services;

namespace Services;

public interface IConfiguracaoConexaoBancoService : IService<ConfiguracaoConexaoBanco>
{
    public Task<IEnumerable<ConfiguracaoConexaoBanco>> GetAllByOperador();
    public ConfiguracaoConexaoBanco Add(ConfiguracaoConexaoBanco configuracaoConexaoBanco);
    public ConfiguracaoConexaoBanco Update(ConfiguracaoConexaoBanco configuracaoConexaoBanco);
}