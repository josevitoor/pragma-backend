using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using TceCore.ACL;

namespace Application.Controllers;

/// <inheritdoc />
/// <summary>
/// Classe controller para configuração de geração
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[AuthorizeTCE]
public class ConfiguracaoGeracaoController : ControllerBase
{
    private readonly IConfiguracaoGeracaoService _configuracaoGeracaoService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor do controller para configuração de geração
    /// </summary>
    public ConfiguracaoGeracaoController(IConfiguracaoGeracaoService configuracaoGeracaoService, IMapper mapper)
    {
        _configuracaoGeracaoService = configuracaoGeracaoService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todas as configurações
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConfiguracaoGeracao>), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var configuracoes = await _configuracaoGeracaoService.GetAllAsync();
        return Ok(configuracoes);
    }


    /// <summary>
    /// Retorna todas as configurações do operador
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("operador")]
    [ProducesResponseType(typeof(IEnumerable<ConfiguracaoGeracaoResponse>), 200)]
    public async Task<IActionResult> GetAllByOperadorAsync()
    {
        var configuracoes = await _configuracaoGeracaoService.GetAllByOperador();
        var result = _mapper.Map<IEnumerable<ConfiguracaoGeracaoResponse>>(configuracoes);
        return Ok(result);
    }

    /// <summary>
    /// Retorna configuração por Id
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ConfiguracaoGeracao), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var configuracao = await _configuracaoGeracaoService.GetByIdAsync(predicate: x => x.IdConfiguracao == id);
        if (configuracao == null)
            return NotFound();

        return Ok(configuracao);
    }

    /// <summary>
    /// Cria nova configuração
    /// </summary>
    /// <param name="configuracaoGeracaoDto"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConfiguracaoGeracao), 200)]
    public IActionResult Create([FromBody] ConfiguracaoGeracaoRequest configuracaoGeracaoDto)
    {
        var configuracaoGeracao = _mapper.Map<ConfiguracaoGeracao>(configuracaoGeracaoDto);

        var result = _configuracaoGeracaoService.Add(configuracaoGeracao);

        return Ok(result);
    }

    /// <summary>
    /// Atualiza configuração por id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="configuracaoGeracaoDto"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ConfiguracaoGeracao), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ConfiguracaoGeracaoRequest configuracaoGeracaoDto)
    {
        var configuracao = await _configuracaoGeracaoService.GetByIdAsync(predicate: x => x.IdConfiguracao == id);

        if (configuracao == null)
            return NotFound();

        _mapper.Map(configuracaoGeracaoDto, configuracao);

        var result = _configuracaoGeracaoService.Update(configuracao);

        return Ok(result);
    }

    /// <summary>
    /// Remove uma configuração por id
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
        var configuracao = await _configuracaoGeracaoService.GetByIdAsync(predicate: x => x.IdConfiguracao == id);

        if (configuracao == null)
            return NotFound();

        _configuracaoGeracaoService.Delete(configuracao);

        return Ok();
    }
}