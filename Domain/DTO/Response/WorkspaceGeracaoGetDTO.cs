using System;
using Domain.Enum;
namespace Domain.DTO.Response;

public class WorkspaceGeracaoGet
{
    public int IdWorkspaceGeracao { get; set; }
    public int IdTipoGeracao { get; set; }
    public string TipoGeracao =>
        IdTipoGeracao switch
        {
            (int)TipoGeracaoEnum.Database => "Database",
            (int)TipoGeracaoEnum.Er => "Modelagem Relacional",
            (int)TipoGeracaoEnum.Sql => "Script SQL",
            _ => "Desconhecido"
        };
    public string Nome { get; set; }
    public string Arquivo { get; set; }
    public DateTime DataInclusao { get; set; }
    public int IdOperadorInclusao { get; set; }
}