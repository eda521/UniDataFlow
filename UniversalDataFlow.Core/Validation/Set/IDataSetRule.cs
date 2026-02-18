namespace UniversalDataFlow.Core.Validation.Set;

using UniversalDataFlow.Core.Data;

public interface IDataSetRule
{
    void Evaluate(
        IReadOnlyDictionary<string, IReadOnlyList<DataRow>> datasets);
}
