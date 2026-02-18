namespace UniversalDataFlow.Core.Validation.Field;

using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Validation.Actions;

public sealed class FieldValidationStep
{
    public IFieldRule Rule { get; }
    public IRuleAction Action { get; }
    public ValidationPolicy Policy { get; }

    public FieldValidationStep(
        IFieldRule rule,
        IRuleAction action,
        ValidationPolicy policy)
    {
        Rule = rule;
        Action = action;
        Policy = policy;
    }
}
