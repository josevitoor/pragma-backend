using Domain.Entities;
using TCE.Base.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services;

public interface IConfiguracaoCaminhosService : IService<ConfiguracaoCaminhos>
{
    public Task<IEnumerable<ConfiguracaoCaminhos>> GetAllByOperadorAsync();
    public Task<ConfiguracaoCaminhos> GetByIdAsync(int id);
    public ConfiguracaoCaminhos Add(ConfiguracaoCaminhos configuracaoCaminhos);
    public ConfiguracaoCaminhos Update(ConfiguracaoCaminhos configuracaoCaminhos);
}