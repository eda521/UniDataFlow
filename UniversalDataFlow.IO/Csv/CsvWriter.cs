using System.Text;
using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.IO.Encoding;

namespace UniversalDataFlow.IO.Csv;

public sealed class CsvWriter
{
    private readonly TextEncoding _encoding;
    private readonly char _separator;

    public CsvWriter(TextEncoding encoding, char separator = ',')
    {
        _encoding = encoding;
        _separator = separator;
    }

    public byte[] Write(
        IEnumerable<DataRow> rows,
        IReadOnlyList<FieldDefinition> fields)
    {
        var sb = new StringBuilder();

        sb.AppendLine(string.Join(_separator, fields.Select(f => f.Name)));

        foreach (var row in rows)
        {
            var values = fields.Select(f =>
                row.Get(f)?.ToString() ?? string.Empty);

            sb.AppendLine(string.Join(_separator, values));
        }

        return _encoding.Encode(sb.ToString());
    }
}
