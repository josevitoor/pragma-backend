using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Filter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
        var key = _configuration.GetValue<string>("Encrypt:Key")
            ?? throw new ValidationException("Chave de criptografia não configurada.");
        var encryptService = new EncryptPasswordService(key);

        var configuracoes = await GetAllAsync(predicate: x => x.IdOperadorInclusao == int.Parse(tokenInfo.IdOperador));

        foreach (var config in configuracoes)
        {
            config.Senha = encryptService.Decrypt(config.Senha);
        }

        return configuracoes;
    }

    public async Task<ConfiguracaoConexaoBanco> GetByIdAsync(int id)
    {
        var key = _configuration.GetValue<string>("Encrypt:Key")
            ?? throw new ValidationException("Chave de criptografia não configurada.");
        var encryptService = new EncryptPasswordService(key);

        var config = await GetByIdAsync(predicate: x => x.IdConfiguracaoConexaoBanco == id);

        config.Senha = encryptService.Decrypt(config.Senha);

        return config;
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
        var key = _configuration.GetValue<string>("Encrypt:Key")
          ?? throw new ValidationException("Chave de criptografia não configurada.");
        var encryptService = new EncryptPasswordService(key);
        configuracaoConexaoBanco.Senha = encryptService.Encrypt(configuracaoConexaoBanco.Senha);
        return Update<ConfiguracaoConexaoBancoValidator>(configuracaoConexaoBanco);
    }

    public async Task ValidateConnection(ConnectionFilter filter)
    {
        var connectionString = $"Data Source={filter.Servidor},{filter.Porta};uid={filter.Usuario};password={filter.Senha};Initial Catalog={filter.BaseDados};";

        var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var dbContext = new DynamicDbContext(optionsBuilder.Options);

        var canConnect = await dbContext.Database.CanConnectAsync();

        if (!canConnect)
            throw new ValidationException("Não foi possível conectar ao banco de dados com os dados fornecidos.");
    }
}