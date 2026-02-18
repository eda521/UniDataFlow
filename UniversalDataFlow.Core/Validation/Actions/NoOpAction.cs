using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Validation;

namespace UniversalDataFlow.Core.Validation.Actions;

/// <summary>
/// Null-object implementation of IRuleAction.
/// Does nothing when rule fails.
/// </summary>
public sealed class NoOpAction : IRuleAction
{
    public static readonly NoOpAction Instance = new();

    private NoOpAction()
    {
    }

    public ActionResult Handle(DataRow row, RuleResult result)
    {
        // intentionally no-op
        return ActionResult.Continue();
    }
}
