namespace Domain.Filter;

public class PaginateFilter
{
    public int Start { get; set; } = 0;

    public int Length { get; set; } = 10;

    public bool ServerSide { get; set; }
}
