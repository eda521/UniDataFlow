using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;

namespace UniversalDataFlow.IO.Excel;

public sealed class ExcelSource
{
    private readonly string _filePath;

    public ExcelSource(string filePath)
    {
        _filePath = filePath;
    }

    public Dictionary<string, IReadOnlyList<DataRow>> LoadSelected(
        IEnumerable<(string DatasetName, string SheetName, FieldRegistry Fields)> mappings)
    {
        var result = new Dictionary<string, IReadOnlyList<DataRow>>();

        using var stream = File.OpenRead(_filePath);
        IWorkbook workbook = new XSSFWorkbook(stream);

        foreach (var map in mappings)
        {
            var sheet = workbook.GetSheet(map.SheetName);

            if (sheet == null)
                throw new InvalidOperationException(
                    $"Sheet '{map.SheetName}' not found in Excel file.");

            var dataset = LoadSheet(sheet, map.Fields);

            result[map.DatasetName] = dataset;
        }

        return result;
    }

    private static IReadOnlyList<DataRow> LoadSheet(ISheet sheet, FieldRegistry fields)
    {
        var formatter = new DataFormatter();
        var rows = new List<DataRow>();

        if (sheet.PhysicalNumberOfRows == 0)
            return rows;

        var headerRow = sheet.GetRow(sheet.FirstRowNum);
        if (headerRow == null)
            return rows;

        var headerIndex = new Dictionary<string, int>(StringComparer.Ordinal);
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            var name = formatter.FormatCellValue(headerRow.GetCell(i));
            if (!string.IsNullOrWhiteSpace(name))
                headerIndex[name] = i;
        }

        for (int r = sheet.FirstRowNum + 1; r <= sheet.LastRowNum; r++)
        {
            var sheetRow = sheet.GetRow(r);
            if (sheetRow == null)
                continue;

            var dataRow = new DataRow();

            foreach (var field in fields.All)
            {
                object? value = null;
                if (headerIndex.TryGetValue(field.Name, out var columnIndex))
                {
                    var cell = sheetRow.GetCell(columnIndex);
                    var raw = formatter.FormatCellValue(cell);

                    value = field.DataType == typeof(int)
                        ? int.TryParse(raw, out var v) ? v : null
                        : raw;
                }

                dataRow.Set(field, value);
            }

            rows.Add(dataRow);
        }

        return rows;
    }
}
