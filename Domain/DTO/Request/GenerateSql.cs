using System.Collections.Generic;

namespace Domain.DTO.Request;
public class GenerateSqlRequest
{
    public List<TableDto> Tables { get; set; } = new();
    public List<LinkDto> Links { get; set; } = new();
}

public class TableDto
{
    public string Key { get; set; } = string.Empty;
    public List<ColumnDto> Columns { get; set; } = new();
}

public class ColumnDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public bool Pk { get; set; }
    public bool Fk { get; set; }
    public bool Nn { get; set; }
    public bool Uq { get; set; }
    public bool Ai { get; set; }
}

public class LinkDto
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;

    public string FromColumn { get; set; } = string.Empty;
    public string ToColumn { get; set; } = string.Empty;
}