using UniversalDataFlow.Core.Fields;

namespace UniversalDataFlow.Core.Data;

public sealed class DatasetData
{
    public FieldRegistry Fields { get; }
    public List<DataRow> Rows { get; }

    public DatasetData(FieldRegistry fields, List<DataRow> rows)
    {
        Fields = fields;
        Rows = rows;
    }
}
