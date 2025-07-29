using System.Threading.Tasks;
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
        _generateService.ValidateProjectStructure(projectApiRootPath, projectClientRootPath, routerFilePath);
        return Ok();
    }
}