namespace Domain.Entities;

public class ConfiguracaoGeracaoRequest
{
    public string BaseDados { get; set; }

    public string Usuario { get; set; }

    public string Senha { get; set; }

    public string Servidor { get; set; }

    public int Porta { get; set; }

    public string CaminhoApi { get; set; }

    public string CaminhoCliente { get; set; }

    public string CaminhoArquivoRota { get; set; }
}