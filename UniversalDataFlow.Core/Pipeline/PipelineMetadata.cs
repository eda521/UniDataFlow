using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Validation.Field;
using UniversalDataFlow.Core.Validation.Row;

namespace UniversalDataFlow.Core.Pipeline;

/// <summary>
/// Immutable runtime metadata for a validated pipeline.
/// Created by PipelineValidator, consumed by PipelineRuntime.
/// </summary>
public sealed class PipelineMetadata
{
    public IReadOnlyList<FieldValidationStep> FieldValidations { get; }
    public IReadOnlyList<RowValidationStep> RowValidations { get; }

    /// <summary>
    /// Diagnostics collector shared for the whole pipeline execution.
    /// </summary>
    public DiagnosticsCollector Diagnostics { get; }

    public PipelineMetadata(
        IReadOnlyList<FieldValidationStep> fieldValidations,
        IReadOnlyList<RowValidationStep> rowValidations,
        DiagnosticsCollector diagnostics)
    {
        FieldValidations = fieldValidations
            ?? throw new ArgumentNullException(nameof(fieldValidations));

        RowValidations = rowValidations
            ?? throw new ArgumentNullException(nameof(rowValidations));

        Diagnostics = diagnostics
            ?? throw new ArgumentNullException(nameof(diagnostics));
    }
}
