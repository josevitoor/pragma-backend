using System.Collections.Generic;
using Domain.Filter;

namespace Domain.DTO.Request;

public class ExecuteScriptDTO
{
    public ConnectionFilter Filter { get; set; }
    public string Script { get; set; }
    public List<string> Tabelas { get; set; }
}