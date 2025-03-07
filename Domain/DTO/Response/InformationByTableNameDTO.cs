namespace Domain.DTO.Response;
public class InformationByTableName
{
    public string ColumnName { get; set; }
    public string DataType { get; set; }
    public string IsNullable { get; set; }
    public int? MaxLength { get; set; }
}