using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper.Comunicacao;

public class ConfiguracaoGeracaoMappingProfile : Profile
{

    public ConfiguracaoGeracaoMappingProfile()
    {

        CreateMap<ConfiguracaoGeracao, ConfiguracaoGeracaoRequest>().ReverseMap();

    }
}
