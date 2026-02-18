using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Diagnostics;

namespace UniversalDataFlow.Core.Runtime;

/// <summary>
/// Result of pipeline execution.
/// Contains accepted and rejected rows,
/// collected diagnostics, and abort flag.
/// </summary>
public sealed class PipelineResult
{
    public IReadOnlyList<DataRow> AcceptedRows { get; }
    public IReadOnlyList<DataRow> RejectedRows { get; }

    public DiagnosticsCollector Diagnostics { get; }

    /// <summary>
    /// True if pipeline execution was aborted
    /// due to StopJob policy.
    /// </summary>
    public bool Aborted { get; }

    public PipelineResult(
        IReadOnlyList<DataRow> acceptedRows,
        IReadOnlyList<DataRow> rejectedRows,
        DiagnosticsCollector diagnostics,
        bool aborted)
    {
        AcceptedRows = acceptedRows
            ?? throw new ArgumentNullException(nameof(acceptedRows));

        RejectedRows = rejectedRows
            ?? throw new ArgumentNullException(nameof(rejectedRows));

        Diagnostics = diagnostics
            ?? throw new ArgumentNullException(nameof(diagnostics));

        Aborted = aborted;
    }
}
