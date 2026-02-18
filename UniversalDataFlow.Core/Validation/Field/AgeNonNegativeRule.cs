namespace UniversalDataFlow.Core.Validation.Field;

using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Validation.Common;

public sealed class AgeNonNegativeRule : IFieldRule
{
    public FieldDefinition Field { get; }

    public AgeNonNegativeRule(FieldDefinition field)
    {
        Field = field;
    }

    public RuleResult Evaluate(object? value)
    {
        if (value is int age && age < 0)
            return new RuleResult(
                isValid: false,
                message: $"AGE_NEGATIVE: Age {age} is negative",
                severity: DiagnosticLevel.Error);


        return RuleResult.Ok();
    }
}
