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
    /// <param name="generateBackendFilter"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("backend-files")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GenerateBackendCrudFiles([FromBody] GenerateBackendFilter generateBackendFilter)
    {
        await _generateService.GenerateBackendCrudFiles(generateBackendFilter);
        return Ok();
    }

    /// <summary>
    /// Validar caminho para geração dos arquivos
    /// </summary>
    /// <param name="projectRootPath"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("validate-structure")]
    public IActionResult ValidateStructure([FromQuery] string projectRootPath)
    {
        _generateService.ValidateProjectStructure(projectRootPath);
        return Ok();
    }
}