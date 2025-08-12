using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Filter;
using Microsoft.AspNetCore.Mvc;
using Services;
using TceCore.ACL;

namespace Application.Controllers;

/// <inheritdoc />
/// <summary>
/// Classe controller para configuração de conexão com o banco de dados
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[AuthorizeTCE]
public class ConfiguracaoConexaoBancoController : ControllerBase
{
    private readonly IConfiguracaoConexaoBancoService _configuracaoConexaoBancoService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor do controller para configuração de conexão com o banco de dados
    /// </summary>
    public ConfiguracaoConexaoBancoController(IConfiguracaoConexaoBancoService configuracaoConexaoBancoService, IMapper mapper)
    {
        _configuracaoConexaoBancoService = configuracaoConexaoBancoService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todas as configurações de conexão com o banco de dados
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConfiguracaoConexaoBanco>), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var configuracoes = await _configuracaoConexaoBancoService.GetAllAsync();
        return Ok(configuracoes);
    }


    /// <summary>
    /// Retorna todas as configurações de conexão com o banco de dados do operador 
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("operador")]
    [ProducesResponseType(typeof(IEnumerable<ConfiguracaoConexaoBancoResponse>), 200)]
    public async Task<IActionResult> GetAllByOperadorAsync()
    {
        var configuracoes = await _configuracaoConexaoBancoService.GetAllByOperador();
        var result = _mapper.Map<IEnumerable<ConfiguracaoConexaoBancoResponse>>(configuracoes);
        return Ok(result);
    }

    /// <summary>
    /// Retorna configuração de conexão com o banco de dados por Id
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ConfiguracaoConexaoBanco), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var configuracao = await _configuracaoConexaoBancoService.GetByIdAsync(id);
        if (configuracao == null)
            return NotFound();

        return Ok(configuracao);
    }

    /// <summary>
    /// Cria nova configuração de conexão com o banco de dados
    /// </summary>
    /// <param name="configuracaoGeracaoDto"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConfiguracaoConexaoBanco), 200)]
    public IActionResult Create([FromBody] ConfiguracaoConexaoBancoRequest configuracaoGeracaoDto)
    {
        var configuracaoGeracao = _mapper.Map<ConfiguracaoConexaoBanco>(configuracaoGeracaoDto);

        var result = _configuracaoConexaoBancoService.Add(configuracaoGeracao);

        return Ok(result);
    }

    /// <summary>
    /// Atualiza configuração de conexão com o banco de dados por id 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="configuracaoConexaoBancoDto"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ConfiguracaoConexaoBanco), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ConfiguracaoConexaoBancoRequest configuracaoConexaoBancoDto)
    {
        var configuracao = await _configuracaoConexaoBancoService.GetByIdAsync(predicate: x => x.IdConfiguracaoConexaoBanco == id);

        if (configuracao == null)
            return NotFound();

        _mapper.Map(configuracaoConexaoBancoDto, configuracao);

        var result = _configuracaoConexaoBancoService.Update(configuracao);

        return Ok(result);
    }

    /// <summary>
    /// Remove uma configuração de conexão com o banco de dados por id
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var configuracao = await _configuracaoConexaoBancoService.GetByIdAsync(predicate: x => x.IdConfiguracaoConexaoBanco == id);

        if (configuracao == null)
            return NotFound();

        _configuracaoConexaoBancoService.Delete(configuracao);

        return Ok();
    }

    /// <summary>
    /// Validar conexão do banco de dados
    /// </summary>
    /// <param name="filter"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("validate-connection")]
    [ProducesResponseType(typeof(IEnumerable<Information>), 200)]
    public async Task<IActionResult> ValidateConnection([FromQuery] ConnectionFilter filter)
    {
        await _configuracaoConexaoBancoService.ValidateConnection(filter);

        return Ok();
    }
}