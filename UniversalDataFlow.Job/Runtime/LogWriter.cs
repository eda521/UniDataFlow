using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Job.Spec;

namespace UniversalDataFlow.Job.Runtime;

public sealed class LogWriter
{
    public void Write(LogSpec spec, DiagnosticsCollector diagnostics)
    {
        var lines = diagnostics.Messages
            .Where(m => ShouldWrite(m.Level, spec.Level))
            .Select(m => $"{m.Level}: {m.Message}");

        File.WriteAllLines(spec.File, lines);
    }

    private static bool ShouldWrite(
        DiagnosticLevel level,
        string minLevel)
    {
        return level >= Enum.Parse<DiagnosticLevel>(minLevel, true);
    }
}
