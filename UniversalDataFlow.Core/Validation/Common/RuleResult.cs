using UniversalDataFlow.Core.Diagnostics;

public sealed class RuleResult
{
    public bool IsValid { get; }

    public string Message { get; }

    public DiagnosticLevel Severity { get; }

    public RuleResult(
        bool isValid,
        string message,
        DiagnosticLevel severity)
    {
        IsValid = isValid;
        Message = message;
        Severity = severity;
    }

    public static RuleResult Ok()
        => new(true, string.Empty, DiagnosticLevel.Info);
}
