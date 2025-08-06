using System;
namespace Domain.DTO.Response;

public class ConfiguracaoCaminhosResponse
{
    public int IdConfiguracaoCaminho { get; set; }

    public string CaminhoApi { get; set; }

    public string CaminhoCliente { get; set; }

    public string CaminhoArquivoRota { get; set; }

    public DateTime DataInclusao { get; set; }
}