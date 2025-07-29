using System;

namespace Domain.Entities;

public class ConfiguracaoGeracao
{
    public int IdConfiguracao { get; set; }

    public string BaseDados { get; set; }

    public string Usuario { get; set; }

    public string Senha { get; set; }

    public string Servidor { get; set; }

    public int Porta { get; set; }

    public string CaminhoApi { get; set; }

    public string CaminhoCliente { get; set; }

    public string CaminhoArquivoRota { get; set; }

    public DateTime DataInclusao { get; set; }

    public int IdOperadorInclusao { get; set; }

    public int IdSessao { get; set; }
}