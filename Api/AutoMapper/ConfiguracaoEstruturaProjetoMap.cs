using AutoMapper;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;

namespace Application.AutoMapper;

/// <summary>
/// Classe para o mapeamento de ConfiguracaoEstruturaProjeto
/// </summary>
public class ConfiguracaoEstruturaProjetoMap : Profile
{
    /// <summary>
    /// Construtor do mapeamento de ConfiguracaoEstruturaProjeto
    /// </summary>
    public ConfiguracaoEstruturaProjetoMap()
    {
        CreateMap<ConfiguracaoEstruturaProjetoPost, ConfiguracaoEstruturaProjeto>().ReverseMap();
        CreateMap<ConfiguracaoEstruturaProjetoGet, ConfiguracaoEstruturaProjeto>().ReverseMap();
    }
}