using System;
namespace Domain.Entities;

public class ConfiguracaoCaminhos
{
    public int IdConfiguracaoCaminho { get; set; }

    public string CaminhoApi { get; set; }

    public string CaminhoCliente { get; set; }

    public string CaminhoArquivoRota { get; set; }

    public DateTime DataInclusao { get; set; }

    public int IdOperadorInclusao { get; set; }

    public int IdSessao { get; set; }
}