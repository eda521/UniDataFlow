namespace UniversalDataFlow.Core.Diagnostics;

public sealed class DiagnosticsCollector
{
    private readonly List<DiagnosticMessage> _messages = new();

    public IReadOnlyList<DiagnosticMessage> Messages => _messages;

    public void Add(DiagnosticLevel level, string message)
    {
        _messages.Add(new DiagnosticMessage(level, message));
    }

    // Pohodlné helpery – VOLITELNÉ
    public void Info(string message)
        => Add(DiagnosticLevel.Info, message);

    public void Warning(string message)
        => Add(DiagnosticLevel.Warning, message);

    public void Error(string message)
        => Add(DiagnosticLevel.Error, message);
}
