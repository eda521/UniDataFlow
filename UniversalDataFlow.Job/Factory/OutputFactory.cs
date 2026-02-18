using System.Text;
using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.IO.Csv;
using UniversalDataFlow.IO.Encoding;
using UniversalDataFlow.IO.Output;
using UniversalDataFlow.Job.Spec;

namespace UniversalDataFlow.Job.Factory;

public static class OutputFactory
{
    public static void WriteOutputs(
        JobSpec spec,
        IReadOnlyDictionary<string, IReadOnlyList<DataRow>> datasets,
        IReadOnlyDictionary<string, FieldRegistry> registries)
    {
        foreach (var (name, output) in spec.Outputs)
        {
            var rows = datasets[output.Source];
            var registry = registries[output.Source];

            var fields = output.Fields
                .Select(f => registry.Get(f))
                .ToList();

            var encoding = new TextEncoding(
                Encoding.GetEncoding(output.Encoding));

            var separator = CsvSeparator.Resolve(output.Separator);

            new CsvOutputWriter().Write(
                output.File,
                rows,
                fields,
                encoding,
                separator);
        }
    }
}
