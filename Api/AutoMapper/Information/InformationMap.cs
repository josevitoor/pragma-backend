using AutoMapper;
using Domain.DTO.Response;
using Domain.Entities;

namespace Application.AutoMapper;

/// <summary>
/// Classe para o mapeamento de Information
/// </summary>
public class InformationMap : Profile
{
    /// <summary>
    /// Construtor do mapeamento de Information
    /// </summary>
    public InformationMap()
    {
        CreateMap<Information, InformationByTableName>().ReverseMap();
    }
}
