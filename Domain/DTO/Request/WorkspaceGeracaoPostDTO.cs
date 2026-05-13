using System;
namespace Domain.DTO.Request;
public class WorkspaceGeracaoPost
{
    public int IdTipoGeracao { get; set; }
    public string Nome { get; set; }
    public string Arquivo { get; set; }
    public DateTime DataInclusao { get; set; }
    public int IdOperadorInclusao { get; set; }
}