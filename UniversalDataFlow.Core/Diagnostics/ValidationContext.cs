namespace UniversalDataFlow.Core.Diagnostics;

public sealed class ValidationContext
{
    public DiagnosticsCollector Diagnostics { get; }
    public DiagnosticLevel Severity { get; }
    public ValidationPolicy Policy { get; }

    public ValidationContext(
        DiagnosticsCollector diagnostics,
        DiagnosticLevel severity,
        ValidationPolicy policy)
    {
        Diagnostics = diagnostics;
        Severity = severity;
        Policy = policy;
    }
}
