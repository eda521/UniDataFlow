using System.Text;
using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Runtime;
using UniversalDataFlow.IO.Csv;
using UniversalDataFlow.IO.Encoding;
using UniversalDataFlow.Job.Factory;
using UniversalDataFlow.Job.Loading;
using UniversalDataFlow.Job.Spec;

namespace UniversalDataFlow.Job.Runtime;

public sealed class JobRunner
{
    public void Run(string jobJsonPath)
    {
        var diagnostics = new DiagnosticsCollector();

        // 1️⃣ Load job spec
        JobSpec jobSpec = JobSpecLoader.Load(jobJsonPath);

        // 2️⃣ Prepare registries + pipelines
        var registries = new Dictionary<string, FieldRegistry>();
        var pipelines = new Dictionary<string, PipelineRuntime>();
        var inputs = new Dictionary<string, IEnumerable<DataRow>>();

        foreach (var (name, source) in jobSpec.Sources)
        {
            // schema
            var fields = FieldRegistryFactory.Create(source);
            registries[name] = fields;

            // pipeline
            pipelines[name] = PipelineFactory.Create(source, fields);

            // CSV input
            var encoding = new TextEncoding(
                Encoding.GetEncoding(source.Encoding));

            var separator = CsvSeparator.Resolve(source.Separator);

            var csv = new CsvSource(encoding, separator);

            var rows = csv.Read(
                File.ReadAllBytes(source.File),
                fields);

            inputs[name] = rows;
        }

        // 3️⃣ Dataset-level rules
        var dataSetRules = jobSpec.DataSetRules
            .Select(r => DataSetRuleFactory.Create(r, registries))
            .ToList();

        // 4️⃣ Run job
        var job = new DataFlowJobRuntime(
            pipelines,
            dataSetRules);

        job.Execute(inputs);

        var datasets = pipelines.ToDictionary(
            p => p.Key,
            p => p.Value.Execute(inputs[p.Key]).AcceptedRows);

        OutputFactory.WriteOutputs(
            jobSpec,
            datasets,
            registries);

        new LogWriter().Write(jobSpec.Log, diagnostics);

    }
}
