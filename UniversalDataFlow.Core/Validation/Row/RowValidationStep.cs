namespace UniversalDataFlow.Core.Validation.Row;

using UniversalDataFlow.Core.Diagnostics;
using UniversalDataFlow.Core.Validation.Actions;
using UniversalDataFlow.Core.Validation.Field;

public sealed class RowValidationStep
{
    public IRowRule Rule { get; }
    public IRuleAction Action { get; }
    public ValidationPolicy Policy { get; }


    public RowValidationStep(IRowRule rule, 
        IRuleAction action,
        ValidationPolicy policy)
    {
        Rule = rule;
        Action = action;
        Policy = policy;
    }
}
