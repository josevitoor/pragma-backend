using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Filter;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Application.Controllers;

/// <inheritdoc />
/// <summary>
/// Classe controller de Information
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class InformationController : ControllerBase
{
    private readonly IInformationService _informationService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor do controller de Information
    /// </summary>
    public InformationController(IInformationService informationService, IMapper mapper)
    {
        _informationService = informationService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna as informações das colunas de todas tabelas do banco de dados.
    /// </summary>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Information>), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        IEnumerable<Information> informations = await _informationService.GetAllAsync();

        return Ok(informations);
    }

    /// <summary>
    /// Retorna as informações das colunas de uma tabela do banco de dados pelo seu nome.
    /// </summary>
    /// <param name="tableName"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("{tableName}")]
    [ProducesResponseType(typeof(IEnumerable<InformationByTableName>), 200)]
    public async Task<IActionResult> GetInfoByTableName([FromRoute] string tableName)
    {
        IEnumerable<Information> informations = await _informationService.GetInfoByTableName(tableName);

        if (informations == null || !informations.Any())
            return NotFound();

        IEnumerable<InformationByTableName> informationsMapped = _mapper.Map<IEnumerable<InformationByTableName>>(informations);

        return Ok(informationsMapped);
    }

    /// <summary>
    /// Retorna as informações das colunas de uma tabela do banco de dados pelo seu nome.
    /// </summary>
    /// <param name="informationFilter"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("generate")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GenerateCrudFiles([FromBody] InformationFilter informationFilter)
    {
        await _informationService.GenerateCrudFiles(informationFilter);
        return Ok();
    }
}