namespace UniversalDataFlow.Core.Validation.Row;

using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Validation.Common;

public interface IRowRule
{
    RuleResult Evaluate(DataRow row);
}
