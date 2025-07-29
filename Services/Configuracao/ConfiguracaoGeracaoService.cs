using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using TCE.Base.Services;
using TCE.Base.UnitOfWork;

namespace Services;

public class ConfiguracaoGeracaoService : BaseService<ConfiguracaoGeracao>, IConfiguracaoGeracaoService
{
    public ConfiguracaoGeracaoService(IUnitOfWork uow) : base(uow)
    {
    }

    public ConfiguracaoGeracao Add(ConfiguracaoGeracao configuracaoGeracao)
    {
        return Add<ConfiguracaoGeracaoValidator>(configuracaoGeracao);
    }

    public ConfiguracaoGeracao Update(ConfiguracaoGeracao configuracaoGeracao)
    {
        return Update<ConfiguracaoGeracaoValidator>(configuracaoGeracao);
    }
}