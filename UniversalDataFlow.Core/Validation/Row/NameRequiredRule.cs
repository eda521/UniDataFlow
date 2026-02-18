using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Validation;

namespace UniversalDataFlow.Core.Validation.Row;

/// <summary>
/// Validates that a given string field is not null or empty.
/// </summary>
public sealed class NameRequiredRule : IRowRule
{
    private readonly FieldDefinition _field;

    public NameRequiredRule(FieldDefinition field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public RuleResult Evaluate(DataRow row)
    {
        var value = row.Get(_field);

        if (value is string s && !string.IsNullOrWhiteSpace(s))
        {
            return RuleResult.Ok();
        }

        return new RuleResult(
            isValid: false,
            message: $"Field '{_field.Name}' is required.",
            severity: DiagnosticLevel.Error);
    }
}
