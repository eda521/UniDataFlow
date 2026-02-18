namespace UniversalDataFlow.Job.Spec;

public sealed class SourceSpec
{
    public string File { get; init; } = string.Empty;
    public string Encoding { get; init; } = "utf-8";

    public string Separator { get; init; } = "semicolon";

    public Dictionary<string, string> Fields { get; init; } = new();
    public PipelineSpecDto Pipeline { get; init; } = new();
}
