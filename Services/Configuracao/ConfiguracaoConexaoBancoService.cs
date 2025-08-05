using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using TCE.Base.Services;
using TCE.Base.Token;
using TCE.Base.UnitOfWork;

namespace Services;

public class ConfiguracaoConexaoBancoService : BaseService<ConfiguracaoConexaoBanco>, IConfiguracaoConexaoBancoService
{
    private readonly IConfiguration _configuration;
    public ConfiguracaoConexaoBancoService(IUnitOfWork uow, IConfiguration configuration) : base(uow)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<ConfiguracaoConexaoBanco>> GetAllByOperador()
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        return await GetAllAsync(predicate: x => x.IdOperadorInclusao == int.Parse(tokenInfo.IdOperador));
    }

    public ConfiguracaoConexaoBanco Add(ConfiguracaoConexaoBanco configuracaoConexaoBanco)
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        configuracaoConexaoBanco.IdOperadorInclusao = int.Parse(tokenInfo.IdOperador);

        var key = _configuration.GetValue<string>("Encrypt:Key")
          ?? throw new ValidationException("Chave de criptografia não configurada.");
        var encryptService = new EncryptPasswordService(key);
        configuracaoConexaoBanco.Senha = encryptService.Encrypt(configuracaoConexaoBanco.Senha);

        return Add<ConfiguracaoConexaoBancoValidator>(configuracaoConexaoBanco);
    }

    public ConfiguracaoConexaoBanco Update(ConfiguracaoConexaoBanco configuracaoConexaoBanco)
    {
        return Update<ConfiguracaoConexaoBancoValidator>(configuracaoConexaoBanco);
    }
}