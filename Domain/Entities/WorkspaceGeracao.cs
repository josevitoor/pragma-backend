using System;
namespace Domain.Entities;
public class WorkspaceGeracao
{
    public int IdWorkspaceGeracao { get; set; }
    public int IdTipoGeracao { get; set; }
    public string Nome { get; set; }
    public string Arquivo { get; set; }
    public DateTime DataInclusao { get; set; }
    public int IdOperadorInclusao { get; set; }
}