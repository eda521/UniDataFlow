using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Runtime;
using UniversalDataFlow.Core.Validation.Actions;

namespace UniversalDataFlow.Core.Validation.Row;

/// <summary>
/// Executes row-level validation steps for a single DataRow.
/// Does not throw; returns RowProcessingResult.
/// </summary>
public sealed class RowValidationExecutor
{
    private readonly IReadOnlyList<RowValidationStep> _steps;
    private readonly DiagnosticsCollector _diagnostics;

    public RowValidationExecutor(
        IReadOnlyList<RowValidationStep> steps,
        DiagnosticsCollector diagnostics)
    {
        _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
    }

    public RowProcessingResult Execute(DataRow row)
    {
        foreach (var step in _steps)
        {
            var result = step.Rule.Evaluate(row);

            if (result.IsValid)
                continue;

            // 1️⃣ Log diagnostic
            _diagnostics.Add(
                result.Severity,
                result.Message);

            // 2️⃣ Apply action (best-effort)
            step.Action.Handle(row, result);

            // 3️⃣ Apply policy
            switch (step.Policy)
            {
                case ValidationPolicy.Continue:
                    continue;

                case ValidationPolicy.SkipRow:
                    return RowProcessingResult.Skipped;

                case ValidationPolicy.StopJob:
                    return RowProcessingResult.StopJob;

                default:
                    throw new InvalidOperationException(
                        $"Unknown validation policy '{step.Policy}'");
            }
        }

        return RowProcessingResult.Accepted;
    }
}
