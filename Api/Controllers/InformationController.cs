using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using TceCore.ACL;

namespace Application.Controllers;

/// <inheritdoc />
/// <summary>
/// Classe controller de Information
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[AuthorizeTCE]
public class InformationController : ControllerBase
{
    private readonly IInformationService _informationService;

    /// <summary>
    /// Construtor do controller de Information
    /// </summary>
    public InformationController(IInformationService informationService, IMapper mapper)
    {
        _informationService = informationService;
    }

    /// <summary>
    /// Realiza conexão no banco de dados com os parâmetros de conexão passados
    /// </summary>
    /// <param name="filter"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("bd-connection")]
    [ProducesResponseType(typeof(IEnumerable<Information>), 200)]
    public async Task<IActionResult> BdConnection([FromBody] ConnectionFilter filter)
    {
        IEnumerable<Information> informations = await _informationService.BdConnection(filter);

        return Ok(informations);
    }
}