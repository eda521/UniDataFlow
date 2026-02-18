namespace UniversalDataFlow.Core.Runtime;

using UniversalDataFlow.Core.Data;

public sealed class RuntimeResult
{
    public IReadOnlyList<DataRow> AcceptedRows { get; }
    public IReadOnlyList<DataRow> RejectedRows { get; }

    public RuntimeResult(
        IReadOnlyList<DataRow> accepted,
        IReadOnlyList<DataRow> rejected)
    {
        AcceptedRows = accepted;
        RejectedRows = rejected;
    }
}
