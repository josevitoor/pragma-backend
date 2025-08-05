using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper.Comunicacao;

public class ConfiguracaoConexaoBancoMappingProfile : Profile
{

    public ConfiguracaoConexaoBancoMappingProfile()
    {
        CreateMap<ConfiguracaoConexaoBanco, ConfiguracaoConexaoBancoRequest>().ReverseMap();
        CreateMap<ConfiguracaoConexaoBanco, ConfiguracaoConexaoBancoResponse>().ReverseMap();
    }
}
