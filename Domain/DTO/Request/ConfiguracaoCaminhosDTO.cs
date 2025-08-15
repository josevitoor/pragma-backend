using Domain.Entities;

namespace Domain.DTO.Request;

public class ConfiguracaoCaminhosRequest
{
    public string CaminhoApi { get; set; }

    public string CaminhoCliente { get; set; }

    public ConfiguracaoEstruturaProjeto ConfiguracaoEstruturaProjeto { get; set; }
}