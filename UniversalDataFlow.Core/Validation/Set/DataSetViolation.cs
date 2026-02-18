namespace UniversalDataFlow.Core.Validation.Set;

using UniversalDataFlow.Core.Data;

public sealed class DataSetViolation
{
    public DataRow Row { get; }
    public string Code { get; }
    public string Message { get; }
}
