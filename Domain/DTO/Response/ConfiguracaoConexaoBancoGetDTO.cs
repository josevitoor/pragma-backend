using System;

namespace Domain.Entities;

public class ConfiguracaoConexaoBancoResponse
{
    public int IdConfiguracaoConexaoBanco { get; set; }

    public string BaseDados { get; set; }

    public string Usuario { get; set; }

    public string Servidor { get; set; }

    public int Porta { get; set; }

    public string Senha { get; set; }

    public DateTime DataInclusao { get; set; }
}