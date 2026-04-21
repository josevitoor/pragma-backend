using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Filter;
using Microsoft.AspNetCore.Mvc;
using Services;
using TceCore.ACL;

namespace Application.Controllers;

/// <inheritdoc />
/// <summary>
/// Classe controller para geração de código
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[AuthorizeTCE]
public class GenerateController : ControllerBase
{
    private readonly IGenerateService _generateService;

    /// <summary>
    /// Construtor do controller de Generate
    /// </summary>
    public GenerateController(IGenerateService generateService)
    {
        _generateService = generateService;
    }

    /// <summary>
    /// Gerar código baseado em template
    /// </summary>
    /// <param name="generateFilter"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("generate-files")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GenerateCrudFiles([FromBody] GenerateFilter generateFilter)
    {
        await _generateService.GenerateCrudFiles(generateFilter);
        return Ok();
    }

    /// <summary>
    /// Gerar script SQL baseado em dados fornecidos
    /// </summary>
    /// <param name="generateSqlRequest"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("generate-sql")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GenerateSql([FromBody] GenerateSqlRequest generateSqlRequest)
    {
        var sql = _generateService.GenerateSql(generateSqlRequest);
        return Ok(new { Sql = sql });
    }
}