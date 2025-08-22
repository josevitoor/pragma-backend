using System;
using System.Collections.Generic;
namespace Domain.Entities;

public class ConfiguracaoEstruturaProjeto
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

    public string ClientServices { get; set; }

    public string ApiImportBaseService { get; set; }

    public string ApiImportUOW { get; set; }

    public string ApiImportPaginate { get; set; }

    public string ClientModels { get; set; }

    public string ClientModulos { get; set; }

    public string ClientArquivoRotas { get; set; }

    public DateTime DataInclusao { get; set; }

    public int IdOperadorInclusao { get; set; }

    public ICollection<ConfiguracaoCaminhos> ConfiguracaoCaminhos { get; set; }
}