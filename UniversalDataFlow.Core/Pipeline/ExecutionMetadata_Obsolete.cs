using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Transformations;
using UniversalDataFlow.Core.Validation.Field;
using UniversalDataFlow.Core.Validation.Row;
using UniversalDataFlow.Core.Validation.Set;

namespace UniversalDataFlow.Core.Pipeline;

public sealed class ExecutionMetadata_Obsolete
{
    public IReadOnlyList<ITransformation> OrderedTransformations { get; }

    public IReadOnlyList<FieldValidationStep> FieldValidations { get; }
    public IReadOnlyList<RowValidationStep> RowValidations { get; }
    public IReadOnlyList<IDataSetRule> DataSetRules { get; }

    public ExecutionMetadata_Obsolete(
        IReadOnlyList<ITransformation> orderedTransformations,
        IReadOnlyList<FieldValidationStep> fieldValidations,
        IReadOnlyList<RowValidationStep> rowValidations,
        IReadOnlyList<IDataSetRule> dataSetRules)
    {
        OrderedTransformations = orderedTransformations;
        FieldValidations = fieldValidations;
        RowValidations = rowValidations;
        DataSetRules = dataSetRules;
    }
}
