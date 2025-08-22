using System;
namespace Domain.Entities;

public class ConfiguracaoCaminhos
{
    public int IdConfiguracaoCaminho { get; set; }

    public int IdConfiguracaoEstrutura { get; set; }

    public string CaminhoApi { get; set; }

    public string CaminhoCliente { get; set; }

    public DateTime DataInclusao { get; set; }

    public int IdOperadorInclusao { get; set; }

    public ConfiguracaoEstruturaProjeto ConfiguracaoEstruturaProjeto { get; set; }
}