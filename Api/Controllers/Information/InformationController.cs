using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Application.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class InformationController : ControllerBase
{
    private readonly IInformationService _informationService;

    public InformationController(IInformationService informationService)
    {
        _informationService = informationService;
    }

    /// <summary>
    /// Retorna todos as informações das colunas das tabelas do banco de dados.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var information = await _informationService.GetAllAsync();

        return Ok(information);
    }
}