
using AutoMapper;

namespace Application.AutoMapper;
/// <summary>
/// Classe geral de mapeamento
/// </summary>
public static class ConfigureMap
{
    /// <summary>
    /// Configurar mapeamentos
    /// </summary>
    public static IMapper Configure()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
        });

        IMapper mapper = mapperConfig.CreateMapper();
        return mapper;
    }
}
