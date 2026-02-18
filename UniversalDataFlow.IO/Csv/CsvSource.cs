using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.IO.Encoding;

namespace UniversalDataFlow.IO.Csv;

public sealed class CsvSource
{
    private readonly TextEncoding _encoding;
    private readonly char _separator;

    public CsvSource(TextEncoding encoding, char separator)
    {
        _encoding = encoding;
        _separator = separator;
    }

    public IEnumerable<DataRow> Read(
        byte[] rawData,
        FieldRegistry fields)
    {
        var text = _encoding.Decode(rawData);
        var lines = text.Split(
            new[] { "\r\n", "\n" },
            StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
            yield break;

        var headers = lines[0].Split(_separator);
        var fieldMap = headers
            .Select(h => fields.Get(h))
            .ToArray();

        foreach (var line in lines.Skip(1))
        {
            var values = line.Split(_separator);
            var row = new DataRow();

            for (int i = 0; i < fieldMap.Length && i < values.Length; i++)
            {
                var field = fieldMap[i];
                var raw = values[i];

                object? value = field.DataType == typeof(int)
                    ? int.TryParse(raw, out var v) ? v : null
                    : raw;

                row.Set(field, value);
            }

            yield return row;
        }
    }
}
