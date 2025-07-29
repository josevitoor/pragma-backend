using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using TCE.Base.Services;
using TCE.Base.Token;
using TCE.Base.UnitOfWork;

namespace Services;

public class ConfiguracaoGeracaoService : BaseService<ConfiguracaoGeracao>, IConfiguracaoGeracaoService
{
    private readonly IConfiguration _configuration;
    public ConfiguracaoGeracaoService(IUnitOfWork uow, IConfiguration configuration) : base(uow)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<ConfiguracaoGeracao>> GetAllByOperador()
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        return await GetAllAsync(predicate: x => x.IdOperadorInclusao == int.Parse(tokenInfo.IdOperador));
    }

    public ConfiguracaoGeracao Add(ConfiguracaoGeracao configuracaoGeracao)
    {
        var tokenInfo = new TokenInfo(_tokenInfo);
        configuracaoGeracao.IdOperadorInclusao = int.Parse(tokenInfo.IdOperador);

        var key = _configuration.GetValue<string>("Encrypt:Key")
          ?? throw new ValidationException("Chave de criptografia não configurada.");
        var encryptService = new EncryptPasswordService(key);
        configuracaoGeracao.Senha = encryptService.Encrypt(configuracaoGeracao.Senha);

        return Add<ConfiguracaoGeracaoValidator>(configuracaoGeracao);
    }

    public ConfiguracaoGeracao Update(ConfiguracaoGeracao configuracaoGeracao)
    {
        return Update<ConfiguracaoGeracaoValidator>(configuracaoGeracao);
    }
}