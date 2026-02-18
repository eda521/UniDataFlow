namespace UniversalDataFlow.Core.Validation.Set;

using UniversalDataFlow.Core.Data;


public sealed class DataSetRuleResult
{
    public bool IsValid { get; }
    public IReadOnlyList<DataSetViolation> Violations { get; }
}
