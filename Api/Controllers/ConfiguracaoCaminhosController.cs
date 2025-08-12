using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Domain.Entities;
using Domain.DTO.Response;
using Domain.DTO.Request;
using AutoMapper;
using TceCore.ACL;

namespace Application.Controllers;

/// <summary>
/// Controller para gerenciamento de configurações de caminho do projeto
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[AuthorizeTCE]
public class ConfiguracaoCaminhosController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IConfiguracaoCaminhosService _service;

    /// <summary>
    /// Construtor do Controller de configurações de caminho do projeto
    /// </summary>
    public ConfiguracaoCaminhosController(IMapper mapper, IConfiguracaoCaminhosService service)
    {
        _mapper = mapper;
        _service = service;
    }

    /// <summary>
    /// Buscar todos os registros de configurações de caminho do projeto
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConfiguracaoCaminhosResponse>), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        IEnumerable<ConfiguracaoCaminhos> configuracaoCaminhos = await _service.GetAllAsync();
        IEnumerable<ConfiguracaoCaminhosResponse> result = _mapper.Map<IEnumerable<ConfiguracaoCaminhosResponse>>(configuracaoCaminhos);
        return Ok(result);
    }

    /// <summary>
    /// Buscar todos os registros de configurações de caminho do projeto por operador
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("operador")]
    [ProducesResponseType(typeof(IEnumerable<ConfiguracaoCaminhosResponse>), 200)]
    public async Task<IActionResult> GetAllByOperadorAsync()
    {
        IEnumerable<ConfiguracaoCaminhos> configuracaoCaminhos = await _service.GetAllByOperadorAsync();
        IEnumerable<ConfiguracaoCaminhosResponse> result = _mapper.Map<IEnumerable<ConfiguracaoCaminhosResponse>>(configuracaoCaminhos);
        return Ok(result);
    }

    /// <summary>
    /// Buscar registro de configurações de caminho do projeto por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ConfiguracaoCaminhosResponse), 200)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        ConfiguracaoCaminhos configuracaoCaminhos = await _service.GetByIdAsync(id);
        if (configuracaoCaminhos == null)
            return NotFound();

        ConfiguracaoCaminhosResponse result = _mapper.Map<ConfiguracaoCaminhosResponse>(configuracaoCaminhos);
        return Ok(result);
    }

    /// <summary>
    /// Criar novo registro de configurações de caminho do projeto
    /// </summary>
    /// <param name="dto">Dados para criação</param>
    /// <response code="201">Criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConfiguracaoCaminhos), 201)]
    public IActionResult Create([FromBody] ConfiguracaoCaminhosRequest dto)
    {
        ConfiguracaoCaminhos configuracaoCaminhos = _mapper.Map<ConfiguracaoCaminhos>(dto);
        ConfiguracaoCaminhos result = _service.Add(configuracaoCaminhos);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Atualizar registro de configurações de caminho do projeto por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <param name="dto">Dados para atualização</param>
    /// <response code="200">Atualizado com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Registro não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ConfiguracaoCaminhos), 200)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ConfiguracaoCaminhosRequest dto)
    {
        ConfiguracaoCaminhos configuracaoCaminhos = await _service.GetByIdAsync(id);
        if (configuracaoCaminhos == null)
            return NotFound();

        ConfiguracaoCaminhos configuracaoCaminhosUpdate = _mapper.Map(dto, configuracaoCaminhos);
        var result = _service.Update(configuracaoCaminhosUpdate);
        return Ok(result);
    }

    /// <summary>
    /// Excluir registro de configurações de caminho do projeto por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <response code="204">Excluído com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Registro não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        ConfiguracaoCaminhos configuracaoCaminhos = await _service.GetByIdAsync(id);
        if (configuracaoCaminhos == null)
            return NotFound();

        _service.Delete(configuracaoCaminhos);
        return NoContent();
    }

    /// <summary>
    /// Validar caminho para geração dos arquivos
    /// </summary>
    /// <param name="projectApiRootPath"></param>
    /// <param name="projectClientRootPath"></param>
    /// <param name="routerFilePath"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("validate-structure")]
    [ProducesResponseType(200)]
    public IActionResult ValidateStructure([FromQuery] string projectApiRootPath, [FromQuery] string projectClientRootPath, [FromQuery] string routerFilePath)
    {
        _service.ValidateProjectStructure(projectApiRootPath, projectClientRootPath, routerFilePath);
        return Ok();
    }
}