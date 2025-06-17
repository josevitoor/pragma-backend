using CrossCutting.Util;

namespace Domain.Entities;

public class Information
{
    public string TableName { get; set; }
    public string ColumnName { get; set; }
    public string DataType { get; set; }
    public string IsNullable { get; set; }
    public int? MaxLength { get; set; }
    public string Label => ColumnName.ToLabel();
    public TableConstraint TableConstraint { get; set; }
}