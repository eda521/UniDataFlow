namespace UniversalDataFlow.Core.Diagnostics;

public enum DiagnosticLevel
{
    Info,
    Warning,
    Error
}

public sealed class DiagnosticMessage
{
    public DiagnosticLevel Level { get; }
    public string Message { get; }

    public DiagnosticMessage(DiagnosticLevel level, string message)
    {
        Level = level;
        Message = message;
    }
}
