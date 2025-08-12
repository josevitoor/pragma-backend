using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Filter;
using TCE.Base.Services;

namespace Services;

public interface IConfiguracaoConexaoBancoService : IService<ConfiguracaoConexaoBanco>
{
    public Task<IEnumerable<ConfiguracaoConexaoBanco>> GetAllByOperador();
    public ConfiguracaoConexaoBanco Add(ConfiguracaoConexaoBanco configuracaoConexaoBanco);
    public ConfiguracaoConexaoBanco Update(ConfiguracaoConexaoBanco configuracaoConexaoBanco);
    public Task ValidateConnection(ConnectionFilter filter);
    public Task<ConfiguracaoConexaoBanco> GetByIdAsync(int id);
}