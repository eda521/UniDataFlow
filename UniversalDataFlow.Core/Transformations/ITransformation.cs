namespace UniversalDataFlow.Core.Transformations;

using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Data;

public interface ITransformation
{
    IReadOnlyCollection<FieldDefinition> Inputs { get; }
    IReadOnlyCollection<FieldDefinition> Outputs { get; }

    void Execute(DataRow row);
}
