namespace UniversalDataFlow.Core.Validation.Actions;

using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Validation.Common;

public sealed class SkipRowAction : IRuleAction
{
    public ActionResult Handle(DataRow row, RuleResult violation)
        => ActionResult.SkipRow(violation.Message);
}
