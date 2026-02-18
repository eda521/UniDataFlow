namespace UniversalDataFlow.Job.Spec;

public sealed class OutputSpec
{
    public string Source { get; init; } = string.Empty;   // dataset name
    public string File { get; init; } = string.Empty;
    public string Encoding { get; init; } = "utf-8";

    public string Separator { get; init; } = "tab";

    public List<string> Fields { get; init; } = new();
}
