using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Pipeline;
using UniversalDataFlow.Core.Runtime;
using UniversalDataFlow.Core.Transformations;
using UniversalDataFlow.Core.Validation;
using UniversalDataFlow.Core.Validation.Actions;
using UniversalDataFlow.Core.Validation.Field;
using UniversalDataFlow.Core.Validation.Row;
using UniversalDataFlow.Core.Validation.Set;
using UniversalDataFlow.Job.Spec;

namespace UniversalDataFlow.Job.Factory;

public static partial class PipelineFactory
{
    public static PipelineRuntime Create(
        SourceSpec spec,
        FieldRegistry fields)
    {
        var fieldValidations = CreateFieldValidations(
            spec.Pipeline.FieldValidations, fields);

        var rowValidations = CreateRowValidations(
            spec.Pipeline.RowValidations, fields);

        var pipelineSpec = new PipelineSpec(
            fields,
            new TransformationRegistry(),
            fieldValidations,
            rowValidations,
            dataSetRules: Array.Empty<IDataSetRule>() // dataset rules patří do JobRuntime
        );

        var metadata = new PipelineValidator().Validate(pipelineSpec);
        return new PipelineRuntime(metadata);
    }

    // =============================================================
    // FIELD VALIDATIONS
    // =============================================================

    private static IReadOnlyList<FieldValidationStep> CreateFieldValidations(
        IEnumerable<ValidationSpec> specs,
        FieldRegistry fields)
    {
        var list = new List<FieldValidationStep>();

        foreach (var spec in specs)
        {
            var rule = CreateFieldRule(spec, fields);
            var action = CreateAction(spec, fields);
            var policy = ParsePolicy(spec);

            list.Add(new FieldValidationStep(
                rule,
                action,
                policy));
        }

        return list;
    }

    // =============================================================
    // ROW VALIDATIONS
    // =============================================================

    private static IReadOnlyList<RowValidationStep> CreateRowValidations(
        IEnumerable<ValidationSpec> specs,
        FieldRegistry fields)
    {
        var list = new List<RowValidationStep>();

        foreach (var spec in specs)
        {
            var rule = CreateRowRule(spec, fields);
            var action = CreateAction(spec, fields);
            var policy = ParsePolicy(spec);

            list.Add(new RowValidationStep(
                rule,
                action,
                policy));
        }

        return list;
    }

    // =============================================================
    // RULE FACTORIES
    // =============================================================

    private static IFieldRule CreateFieldRule(
        ValidationSpec spec,
        FieldRegistry fields)
    {
        return spec.Rule switch
        {
            "AgeNonNegative" =>
                new AgeNonNegativeRule(
                    fields.Get(GetParam(spec, "field"))),

            _ => throw new InvalidOperationException(
                $"Unknown field rule '{spec.Rule}'")
        };
    }

    private static IRowRule CreateRowRule(
        ValidationSpec spec,
        FieldRegistry fields)
    {
        return spec.Rule switch
        {
            "NameRequired" =>
                new NameRequiredRule(
                    fields.Get(GetParam(spec, "field"))),

            _ => throw new InvalidOperationException(
                $"Unknown row rule '{spec.Rule}'")
        };
    }

    // =============================================================
    // ACTION FACTORY
    // =============================================================

    private static IRuleAction CreateAction(
        ValidationSpec spec,
        FieldRegistry fields)
    {
        // Action je VOLITELNÁ
        if (string.IsNullOrWhiteSpace(spec.Action))
            return NoOpAction.Instance;

        return spec.Action switch
        {
            "SetZero" =>
                new SetIntFieldToZeroAction(
                    fields.Get(GetParam(spec, "field"))),

            "SkipRow" =>
                new SkipRowAction(),

            _ => throw new InvalidOperationException(
                $"Unknown action '{spec.Action}'")
        };
    }

    // =============================================================
    // POLICY & PARAMS
    // =============================================================

    private static ValidationPolicy ParsePolicy(ValidationSpec spec)
    {
        return Enum.TryParse<ValidationPolicy>(
            spec.Policy,
            ignoreCase: true,
            out var policy)
            ? policy
            : throw new InvalidOperationException(
                $"Unknown validation policy '{spec.Policy}'");
    }

    private static string GetParam(
        ValidationSpec spec,
        string name)
    {
        if (!spec.Params.TryGetValue(name, out var value))
            throw new InvalidOperationException(
                $"Missing parameter '{name}' for rule '{spec.Rule}'");

        return value;
    }
}
