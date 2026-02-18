namespace UniversalDataFlow.Core.Runtime;

using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Pipeline;
using UniversalDataFlow.Core.Validation.Set;

public sealed class DataFlowJobRuntime
{
    private readonly Dictionary<string, PipelineRuntime> _pipelines;
    private readonly IReadOnlyList<IDataSetRule> _dataSetRules;

    public DataFlowJobRuntime(
        Dictionary<string, PipelineRuntime> pipelines,
        IEnumerable<IDataSetRule> dataSetRules)
    {
        _pipelines = pipelines;
        _dataSetRules = dataSetRules.ToList();
    }

    public void Execute(
        Dictionary<string, IEnumerable<DataRow>> inputs)
    {
        // 1️⃣ Spusť každou pipeline
        var results = new Dictionary<string, IReadOnlyList<DataRow>>();

        foreach (var (name, pipeline) in _pipelines)
        {
            var input = inputs[name];
            var result = pipeline.Execute(input);
            results[name] = result.AcceptedRows;
        }

        // 2️⃣ Spusť dataset-level rules
        foreach (var rule in _dataSetRules)
        {
            rule.Evaluate(results);
        }
    }
}
