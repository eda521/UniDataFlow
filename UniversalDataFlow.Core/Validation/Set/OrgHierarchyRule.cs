using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Validation.Set;

public sealed class OrgHierarchyRule : IDataSetRule
{
    private readonly string _orgDataset;
    private readonly FieldDefinition _id;
    private readonly FieldDefinition _parent;
    private readonly int _rootValue;

    public OrgHierarchyRule(
        string orgDataset,
        FieldDefinition id,
        FieldDefinition parent,
        int rootValue = 0)
    {
        _orgDataset = orgDataset;
        _id = id;
        _parent = parent;
        _rootValue = rootValue;
    }

    public void Evaluate(
        IReadOnlyDictionary<string, IReadOnlyList<DataRow>> data)
    {
        var orgs = data[_orgDataset];
        var byId = orgs.ToDictionary(o => o.Get<int>(_id));

        // sirotci (kromě root)
        foreach (var org in orgs)
        {
            var parent = org.Get<int>(_parent);
            if (parent != _rootValue && !byId.ContainsKey(parent))
                throw new InvalidOperationException(
                    $"Org {org.Get<int>(_id)} has orphan parent {parent}");
        }

        // cykly
        foreach (var org in orgs)
            DetectCycle(org, byId, new HashSet<int>());
    }

    private void DetectCycle(
        DataRow current,
        Dictionary<int, DataRow> map,
        HashSet<int> path)
    {
        var id = current.Get<int>(_id);
        if (!path.Add(id))
            throw new InvalidOperationException(
                $"Cycle detected at org {id}");

        var parent = current.Get<int>(_parent);

        if (parent == _rootValue)
            return;

        DetectCycle(map[parent], map, path);
    }
}
