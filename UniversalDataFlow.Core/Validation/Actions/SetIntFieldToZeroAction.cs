namespace UniversalDataFlow.Core.Validation.Actions;

using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Validation.Common;

public sealed class SetIntFieldToZeroAction : IRuleAction
{
    private readonly FieldDefinition _field;

    public SetIntFieldToZeroAction(FieldDefinition field)
    {
        _field = field;
    }

    public ActionResult Handle(DataRow row, RuleResult violation)
    {
        row.Set(_field, 0);
        return ActionResult.Continue();
    }
}
