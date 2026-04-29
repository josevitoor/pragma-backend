using System.Collections.Generic;

namespace Domain.Filter;

public class GenerateBatchRequest
{
    public List<GenerateFilter> Items { get; set; }
}