using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;

namespace UniversalDataFlow.Core.Validation.Set;

public sealed class PersonOrgExistsRule : IDataSetRule
{
    private readonly string _persDataset;
    private readonly string _orgDataset;

    private readonly FieldDefinition _persId;
    private readonly FieldDefinition _orgRef;
    private readonly FieldDefinition _orgId;

    public PersonOrgExistsRule(
        string persDataset,
        string orgDataset,
        FieldDefinition persId,
        FieldDefinition orgRef,
        FieldDefinition orgId)
    {
        _persDataset = persDataset;
        _orgDataset = orgDataset;
        _persId = persId;
        _orgRef = orgRef;
        _orgId = orgId;
    }

    public void Evaluate(
        IReadOnlyDictionary<string, IReadOnlyList<DataRow>> data)
    {
        var orgIds = data[_orgDataset]
            .Select(o => o.Get<int>(_orgId))
            .ToHashSet();

        foreach (var p in data[_persDataset])
        {
            var org = p.Get<int>(_orgRef);
            if (!orgIds.Contains(org))
                throw new InvalidOperationException(
                    $"Person {p.Get<int>(_persId)} references unknown org {org}");
        }
    }
}
