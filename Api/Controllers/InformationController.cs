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
    /// Retorna as informações de todas tabelas com base na conexão de banco de dados passada
    /// </summary>
    /// <param name="filter"></param>
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Information>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] ConnectionFilter filter)
    {
        IEnumerable<Information> informations = await _informationService.GetAllInformations(filter);

        return Ok(informations);
    }
}