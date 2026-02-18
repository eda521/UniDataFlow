using UniversalDataFlow.Job.Spec;

public sealed class JobSpec
{
    public Dictionary<string, SourceSpec> Sources { get; init; } = new();
    public List<DataSetRuleSpec> DataSetRules { get; init; } = new();

    public Dictionary<string, OutputSpec> Outputs { get; init; } = new();
    public LogSpec Log { get; init; } = new();
}
