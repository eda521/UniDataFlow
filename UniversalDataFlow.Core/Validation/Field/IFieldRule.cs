namespace UniversalDataFlow.Core.Validation.Field;

using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Validation.Common;

public interface IFieldRule
{
    FieldDefinition Field { get; }

    RuleResult Evaluate(object? value);
}
