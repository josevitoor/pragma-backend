using System.Collections.Generic;

namespace Domain.Filter;

public class GenerateFilter
{
    public string TableName { get; set; }
    public string EntityName { get; set; }
    public bool IsServerSide { get; set; }
    public bool HasTceBase { get; set; }
    public ICollection<string> TableColumnsFilter { get; set; }
    public GenerateBackendFilter GenerateBackendFilter { get; set; }
    public GenerateFrontendFilter GenerateFrontendFilter { get; set; }
    public ConnectionFilter ConnectionFilter { get; set; }
}