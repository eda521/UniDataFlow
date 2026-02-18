using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Pipeline;
using UniversalDataFlow.Core.Validation;
using UniversalDataFlow.Core.Validation.Actions;
using UniversalDataFlow.Core.Validation.Field;
using UniversalDataFlow.Core.Validation.Row;

namespace UniversalDataFlow.Core.Runtime;

public sealed class PipelineRuntime
{
    private readonly PipelineMetadata _metadata;

    public PipelineRuntime(PipelineMetadata metadata)
    {
        _metadata = metadata;
    }

    public PipelineResult Execute(IEnumerable<DataRow> rows)
    {
        var accepted = new List<DataRow>();
        var rejected = new List<DataRow>();

        foreach (var row in rows)
        {
            var result = ProcessRow(row);

            switch (result)
            {
                case RowProcessingResult.Accepted:
                    accepted.Add(row);
                    break;

                case RowProcessingResult.Skipped:
                    rejected.Add(row);
                    break;

                case RowProcessingResult.StopJob:
                    return new PipelineResult(
                        accepted,
                        rejected,
                        _metadata.Diagnostics,
                        aborted: true);
            }
        }

        return new PipelineResult(
            accepted,
            rejected,
            _metadata.Diagnostics,
            aborted: false);
    }

    // -------------------------------------------------------------

    private RowProcessingResult ProcessRow(DataRow row)
    {
        // 1️⃣ Field-level validations
        foreach (var step in _metadata.FieldValidations)
        {
            var value = row.Get(step.Rule.Field);

            var result = step.Rule.Evaluate(value);

            if (!result.IsValid)
            {
                _metadata.Diagnostics.Add(
                    result.Severity,
                    result.Message);

                var actionResult = step.Action.Handle(row, result);

                if (step.Policy == ValidationPolicy.SkipRow)
                    return RowProcessingResult.Skipped;

                if (step.Policy == ValidationPolicy.StopJob)
                    return RowProcessingResult.StopJob;

                // Continue = jen log + případná úprava dat
            }
        }

        // 2️⃣ Row-level validations
        foreach (var step in _metadata.RowValidations)
        {
            var result = step.Rule.Evaluate(row);

            if (!result.IsValid)
            {
                _metadata.Diagnostics.Add(
                    result.Severity,
                    result.Message);

                var actionResult = step.Action.Handle(row, result);

                if (step.Policy == ValidationPolicy.SkipRow)
                    return RowProcessingResult.Skipped;

                if (step.Policy == ValidationPolicy.StopJob)
                    return RowProcessingResult.StopJob;
            }
        }

        return RowProcessingResult.Accepted;
    }
}
