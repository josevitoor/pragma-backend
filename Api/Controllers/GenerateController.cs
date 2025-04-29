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
    /// <param name="informationFilter"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("generate")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GenerateCrudFiles([FromBody] InformationFilter informationFilter)
    {
        await _generateService.GenerateCrudFiles(informationFilter);
        return Ok();
    }
}