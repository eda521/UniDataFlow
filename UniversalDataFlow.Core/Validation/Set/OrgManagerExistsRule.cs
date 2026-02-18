using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;

namespace UniversalDataFlow.Core.Validation.Set;

public sealed class OrgManagerExistsRule : IDataSetRule
{
    private readonly string _orgDataset;
    private readonly string _persDataset;

    private readonly FieldDefinition _orgId;
    private readonly FieldDefinition _manager;
    private readonly FieldDefinition _persId;

    public OrgManagerExistsRule(
        string orgDataset,
        string persDataset,
        FieldDefinition orgId,
        FieldDefinition manager,
        FieldDefinition persId)
    {
        _orgDataset = orgDataset;
        _persDataset = persDataset;
        _orgId = orgId;
        _manager = manager;
        _persId = persId;
    }

    public void Evaluate(
        IReadOnlyDictionary<string, IReadOnlyList<DataRow>> data)
    {
        var persIds = data[_persDataset]
            .Select(p => p.Get<int>(_persId))
            .ToHashSet();

        foreach (var o in data[_orgDataset])
        {
            var mgr = o.Get<int>(_manager);
            if (!persIds.Contains(mgr))
                throw new InvalidOperationException(
                    $"Org {o.Get<int>(_orgId)} has invalid manager {mgr}");
        }
    }
}
