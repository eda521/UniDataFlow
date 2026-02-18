using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Transformations;
using UniversalDataFlow.Core.Validation.Field;
using UniversalDataFlow.Core.Validation.Row;
using UniversalDataFlow.Core.Validation.Set;

namespace UniversalDataFlow.Core.Pipeline;

public sealed class PipelineSpec
{
    public FieldRegistry Fields { get; }
    public TransformationRegistry Transformations { get; }

    public IReadOnlyList<FieldValidationStep> FieldValidations { get; }
    public IReadOnlyList<RowValidationStep> RowValidations { get; }
    public IReadOnlyList<IDataSetRule> DataSetRules { get; }

    public PipelineSpec(
        FieldRegistry fields,
        TransformationRegistry transformations,
        IEnumerable<FieldValidationStep>? fieldValidations = null,
        IEnumerable<RowValidationStep>? rowValidations = null,
        IEnumerable<IDataSetRule>? dataSetRules = null)
    {
        Fields = fields;
        Transformations = transformations;
        FieldValidations = fieldValidations?.ToList() ?? new List<FieldValidationStep>();
        RowValidations = rowValidations?.ToList() ?? new List<RowValidationStep>();
        DataSetRules = dataSetRules?.ToList() ?? new List<IDataSetRule>();
    }
}
