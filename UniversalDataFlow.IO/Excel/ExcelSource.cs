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

    public Dictionary<string, DatasetData> LoadSelected(
        IEnumerable<(string DatasetName, string SheetName)> mappings)
    {
        var result = new Dictionary<string, DatasetData>();

        using var stream = File.OpenRead(_filePath);
        IWorkbook workbook = new XSSFWorkbook(stream);

        foreach (var map in mappings)
        {
            var sheet = workbook.GetSheet(map.SheetName);

            if (sheet == null)
                throw new InvalidOperationException(
                    $"Sheet '{map.SheetName}' not found in Excel file.");

            var dataset = LoadSheet(sheet);

            result[map.DatasetName] = dataset;
        }

        return result;
    }

    private DatasetData LoadSheet(ISheet sheet)
    {
        var rows = new List<DataRow>();

        if (sheet.PhysicalNumberOfRows == 0)
            return new DatasetData(new FieldRegistry(), rows);

        var headerRow = sheet.GetRow(sheet.FirstRowNum);
        if (headerRow == null)
            return new DatasetData(new FieldRegistry(), rows);

        var registry = new FieldRegistry();

        var fieldDefinitions = new List<FieldDefinition>();

        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            var cell = headerRow.GetCell(i);
            var name = cell?.ToString() ?? $"Column{i}";

            var field = registry.Register(name, typeof(System.String));
            fieldDefinitions.Add(field);
        }

        for (int r = sheet.FirstRowNum + 1; r <= sheet.LastRowNum; r++)
        {
            var sheetRow = sheet.GetRow(r);
            if (sheetRow == null)
                continue;

            var dataRow = new DataRow();

            for (int c = 0; c < fieldDefinitions.Count; c++)
            {
                var cell = sheetRow.GetCell(c);
                var value = cell?.ToString() ?? string.Empty;

                dataRow.Set(fieldDefinitions[c], value);
            }

            rows.Add(dataRow);
        }

        return new DatasetData(registry, rows);
    }
}
