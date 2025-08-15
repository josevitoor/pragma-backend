using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Domain.Entities;
using Domain.DTO.Response;
using Domain.DTO.Request;
using AutoMapper;
using TceCore.ACL;
using System.Linq;
using FluentValidation;

namespace Application.Controllers;

/// <summary>
/// Controller para gerenciamento de ConfiguracaoEstruturaProjeto
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[AuthorizeTCE]
public class ConfiguracaoEstruturaProjetoController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IConfiguracaoEstruturaProjetoService _service;

    /// <summary>
    /// Construtor do Controller de Configuração de Estrutura de Projeto
    /// </summary>
    public ConfiguracaoEstruturaProjetoController(IMapper mapper, IConfiguracaoEstruturaProjetoService service)
    {
        _mapper = mapper;
        _service = service;
    }

    /// <summary>
    /// Buscar todos os registros de Configuração de Estrutura de Projeto
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConfiguracaoEstruturaProjetoGet>), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        IEnumerable<ConfiguracaoEstruturaProjeto> configuracaoEstruturaProjetos = await _service.GetAllAsync();
        IEnumerable<ConfiguracaoEstruturaProjetoGet> result = _mapper.Map<IEnumerable<ConfiguracaoEstruturaProjetoGet>>(configuracaoEstruturaProjetos);
        return Ok(result);
    }

    /// <summary>
    /// Buscar registro de Configuração de Estrutura de Projeto por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ConfiguracaoEstruturaProjetoGet), 200)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto = await _service.GetByIdAsync(id);
        if (configuracaoEstruturaProjeto == null)
            return NotFound();

        ConfiguracaoEstruturaProjetoGet result = _mapper.Map<ConfiguracaoEstruturaProjetoGet>(configuracaoEstruturaProjeto);
        return Ok(result);
    }

    /// <summary>
    /// Criar novo registro de Configuração de Estrutura de Projeto
    /// </summary>
    /// <param name="dto">Dados para criação</param>
    /// <response code="201">Criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConfiguracaoEstruturaProjeto), 201)]
    public IActionResult Create([FromBody] ConfiguracaoEstruturaProjetoPost dto)
    {
        ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto = _mapper.Map<ConfiguracaoEstruturaProjeto>(dto);
        ConfiguracaoEstruturaProjeto result = _service.Add(configuracaoEstruturaProjeto);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Atualizar registro de Configuração de Estrutura de Projeto por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <param name="dto">Dados para atualização</param>
    /// <response code="200">Atualizado com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Registro não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ConfiguracaoEstruturaProjeto), 200)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ConfiguracaoEstruturaProjetoPost dto)
    {
        ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto = await _service.GetByIdAsync(id);
        if (configuracaoEstruturaProjeto == null)
            return NotFound();

        ConfiguracaoEstruturaProjeto configuracaoEstruturaProjetoUpdate = _mapper.Map(dto, configuracaoEstruturaProjeto);
        var result = _service.Update(configuracaoEstruturaProjetoUpdate);
        return Ok(result);
    }

    /// <summary>
    /// Excluir registro de Configuração de Estrutura de Projeto por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <response code="204">Excluído com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Registro não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] IConfiguracaoCaminhosService configuracaoCaminhosService)
    {
        ConfiguracaoEstruturaProjeto configuracaoEstruturaProjeto = await _service.GetByIdAsync(id);
        if (configuracaoEstruturaProjeto == null)
            return NotFound();

        var caminhos = await configuracaoCaminhosService.GetAllAsync(predicate: x => x.IdConfiguracaoEstrutura == configuracaoEstruturaProjeto.IdConfiguracaoEstrutura);

        if (caminhos != null && caminhos.Count() > 0)
        {
            throw new ValidationException("Esta estrutura de projeto possui caminhos vinculados e não pode ser excluída.");
        }

        _service.Delete(configuracaoEstruturaProjeto);
        return NoContent();
    }
}