using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Validation;
using UniversalDataFlow.Core.Validation.Actions;
using UniversalDataFlow.Core.Validation.Field;
using UniversalDataFlow.Core.Validation.Row;

namespace UniversalDataFlow.Core.Pipeline;

/// <summary>
/// Validates PipelineSpec and produces immutable PipelineMetadata
/// used by PipelineRuntime.
/// </summary>
public sealed class PipelineValidator
{
    public PipelineMetadata Validate(PipelineSpec spec)
    {
        if (spec == null)
            throw new ArgumentNullException(nameof(spec));

        // 1️⃣ Validate FieldValidationSteps
        var fieldSteps = ValidateFieldSteps(
            spec.Fields,
            spec.FieldValidations);

        // 2️⃣ Validate RowValidationSteps
        var rowSteps = ValidateRowSteps(
            spec.Fields,
            spec.RowValidations);

        // 3️⃣ Create shared diagnostics collector
        var diagnostics = new DiagnosticsCollector();

        return new PipelineMetadata(
            fieldSteps,
            rowSteps,
            diagnostics);
    }

    // -------------------------------------------------------------

    private static IReadOnlyList<FieldValidationStep> ValidateFieldSteps(
        FieldRegistry fields,
        IEnumerable<FieldValidationStep> steps)
    {
        var list = new List<FieldValidationStep>();

        foreach (var step in steps)
        {
            if (step.Rule == null)
                throw new InvalidOperationException(
                    "Field validation rule is null.");

            if (step.Action == null)
                throw new InvalidOperationException(
                    "Field validation action is null.");

            // Rule must reference a registered field
            if (step.Rule.Field == null)
                throw new InvalidOperationException(
                    "Field rule does not reference a field.");

            if (!fields.All.Contains(step.Rule.Field))
                throw new InvalidOperationException(
                    $"Field rule references unknown field '{step.Rule.Field.Name}'.");

            list.Add(step);
        }

        return list;
    }

    // -------------------------------------------------------------

    private static IReadOnlyList<RowValidationStep> ValidateRowSteps(
        FieldRegistry fields,
        IEnumerable<RowValidationStep> steps)
    {
        var list = new List<RowValidationStep>();

        foreach (var step in steps)
        {
            if (step.Rule == null)
                throw new InvalidOperationException(
                    "Row validation rule is null.");

            if (step.Action == null)
                throw new InvalidOperationException(
                    "Row validation action is null.");

            // Row rules may reference multiple fields internally;
            // validation is structural, not semantic
            list.Add(step);
        }

        return list;
    }
}
