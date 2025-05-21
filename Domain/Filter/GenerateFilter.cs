namespace Domain.Filter;

public class GenerateFilter
{
    public string TableName { get; set; }
    public string EntityName { get; set; }
    public GenerateBackendFilter GenerateBackendFilter { get; set; }
    public GenerateFrontendFilter GenerateFrontendFilter { get; set; }
    public ConnectionFilter ConnectionFilter { get; set; }
}