using System;
namespace Domain.DTO.Response;

public class ConfiguracaoEstruturaProjetoGet
{
    public int IdConfiguracaoEstrutura { get; set; }

    public string NomeEstrutura { get; set; }

    public string ApiDependencyInjectionConfig { get; set; }

    public string ApiConfigureMap { get; set; }

    public string ApiControllers { get; set; }

    public string ApiEntities { get; set; }

    public string ApiMapping { get; set; }

    public string ApiContexts { get; set; }

    public string ApiServices { get; set; }

    public string ClientAppModule { get; set; }

    public string ClientServices { get; set; }

    public string ClientModels { get; set; }

    public string ClientModulos { get; set; }

    public DateTime DataInclusao { get; set; }

    public int IdOperadorInclusao { get; set; }

    public int IdSessao { get; set; }
}