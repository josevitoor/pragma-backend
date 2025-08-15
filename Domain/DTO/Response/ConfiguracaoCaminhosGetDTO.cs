using System;
using Domain.Entities;
namespace Domain.DTO.Response;

public class ConfiguracaoCaminhosResponse
{
    public int IdConfiguracaoCaminho { get; set; }

    public string CaminhoApi { get; set; }

    public string CaminhoCliente { get; set; }

    public DateTime DataInclusao { get; set; }

    public ConfiguracaoEstruturaProjeto ConfiguracaoEstruturaProjeto { get; set; }
}