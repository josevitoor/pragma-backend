namespace Domain.Entities;

public class TableConstraint
{
    public string TableName { get; set; }
    public string ColumnName { get; set; }
    public string ConstraintName { get; set; }
    public ConstraintInfo ConstraintInfo { get; set; }
    public Information Information { get; set; }
}