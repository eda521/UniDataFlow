namespace UniversalDataFlow.Job.Spec;

public sealed class LogSpec
{
    public string File { get; init; } = "job.log";
    public string Level { get; init; } = "Info"; // Info | Warning | Error
}
