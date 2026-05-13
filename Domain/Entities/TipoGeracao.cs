using System;
namespace Domain.Entities;

public class TipoGeracao
{
    public int IdTipoGeracao { get; set; }
    public string Tipo { get; set; }
    public DateTime DataInclusao { get; set; }
    public int IdOperadorInclusao { get; set; }
}