using AutoMapper;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;

namespace Application.AutoMapper;

/// <summary>
/// Classe para o mapeamento de ConfiguracaoCaminhos
/// </summary>
public class ConfiguracaoCaminhosMap : Profile
{
    /// <summary>
    /// Construtor do mapeamento de ConfiguracaoCaminhos
    /// </summary>
    public ConfiguracaoCaminhosMap()
    {
        CreateMap<ConfiguracaoCaminhosRequest, ConfiguracaoCaminhos>().ReverseMap();
        CreateMap<ConfiguracaoCaminhosResponse, ConfiguracaoCaminhos>().ReverseMap();
        
    }
}