namespace UniversalDataFlow.Job.Spec;

public sealed class DataSetRuleSpec
{
    public string Type { get; init; } = string.Empty;
    public Dictionary<string, string> Params { get; init; } = new();
}
