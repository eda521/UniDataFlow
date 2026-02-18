using System.Text;
using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.IO.Encoding;

namespace UniversalDataFlow.IO.Output;

public sealed class CsvOutputWriter
{
    public void Write(
        string filePath,
        IEnumerable<DataRow> rows,
        IReadOnlyList<FieldDefinition> fields,
        TextEncoding encoding,
        char separator)
    {
        var sb = new StringBuilder();

        sb.AppendLine(string.Join(separator, fields.Select(f => f.Name)));

        foreach (var row in rows)
        {
            var values = fields.Select(f =>
                row.Get(f)?.ToString() ?? string.Empty);

            sb.AppendLine(string.Join(separator, values));
        }

        File.WriteAllBytes(filePath, encoding.Encode(sb.ToString()));
    }
}
