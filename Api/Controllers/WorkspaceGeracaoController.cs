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
/// Controller para gerenciamento de Workspace Geracao
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[AuthorizeTCE]
public class WorkspaceGeracaoController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IWorkspaceGeracaoService _service;

    /// <summary>
    /// Construtor do Controller de Workspace Geracao
    /// </summary>
    public WorkspaceGeracaoController(IMapper mapper, IWorkspaceGeracaoService service)
    {
        _mapper = mapper;
        _service = service;
    }

    /// <summary>
    /// Buscar todos os registros de Workspace Geracao
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WorkspaceGeracaoGet>), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        IEnumerable<WorkspaceGeracao> workspaceGeracaos = await _service.GetAllAsync();
        IEnumerable<WorkspaceGeracaoGet> result = _mapper.Map<IEnumerable<WorkspaceGeracaoGet>>(workspaceGeracaos);
        return Ok(result);
    }

    /// <summary>
    /// Buscar todos os registros de Workspace Geracao pelo operador
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("operador")]
    [ProducesResponseType(typeof(IEnumerable<WorkspaceGeracaoGet>), 200)]
    public async Task<IActionResult> GetAllByOperador()
    {
        IEnumerable<WorkspaceGeracao> workspaceGeracaos = await _service.GetAllByOperador();
        IEnumerable<WorkspaceGeracaoGet> result = _mapper.Map<IEnumerable<WorkspaceGeracaoGet>>(workspaceGeracaos);
        return Ok(result);
    }

    /// <summary>
    /// Buscar registro de Workspace Geracao por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WorkspaceGeracaoGet), 200)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        WorkspaceGeracao workspaceGeracao = await _service.GetByIdAsync(id);
        if (workspaceGeracao == null)
            return NotFound();

        WorkspaceGeracaoGet result = _mapper.Map<WorkspaceGeracaoGet>(workspaceGeracao);
        return Ok(result);
    }

    /// <summary>
    /// Criar novo registro de Workspace Geracao
    /// </summary>
    /// <param name="dto">Dados para criação</param>
    /// <response code="200">Criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(WorkspaceGeracao), 200)]
    public IActionResult Create([FromBody] WorkspaceGeracaoPost dto)
    {
        WorkspaceGeracao workspaceGeracao = _mapper.Map<WorkspaceGeracao>(dto);
        WorkspaceGeracao result = _service.Add(workspaceGeracao);
        return Ok(result);
    }

    /// <summary>
    /// Atualizar registro de Workspace Geracao por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <param name="dto">Dados para atualização</param>
    /// <response code="200">Atualizado com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(WorkspaceGeracao), 200)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] WorkspaceGeracaoPost dto)
    {
       WorkspaceGeracao workspaceGeracao = await _service.GetByIdAsync(id);
        if (workspaceGeracao == null)
            return NotFound();

        WorkspaceGeracao workspaceGeracaoUpdate = _mapper.Map(dto, workspaceGeracao);
        var result = _service.Update(workspaceGeracaoUpdate);
        return Ok(result);
    }

    /// <summary>
    /// Excluir registro de Workspace Geracao por ID
    /// </summary>
    /// <param name="id">ID do registro</param>
    /// <response code="204">Sem conteúdo - Excluído com sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        WorkspaceGeracao workspaceGeracao = await _service.GetByIdAsync(id);
        if (workspaceGeracao == null)
            return NotFound();

        _service.Delete(workspaceGeracao);
        return NoContent();
    }
}