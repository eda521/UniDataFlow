namespace UniversalDataFlow.Core.Validation.Actions;

using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Validation.Common;

public interface IRuleAction
{
    ActionResult Handle(DataRow row, RuleResult violation);
}
