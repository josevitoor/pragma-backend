using System.Collections.Generic;
using Domain.DTO.Request;

namespace Domain.Filter;

public class GenerateFrontendFilter
{
    public string ProjectClientPath { get; set; }
    public string RouterPath { get; set; }
    public ICollection<ColumnMap> TableColumnsList { get; set; }
}