using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Validation.Set;
using UniversalDataFlow.Job.Spec;

namespace UniversalDataFlow.Job.Factory;

public static class DataSetRuleFactory
{
    public static IDataSetRule Create(
        DataSetRuleSpec spec,
        IReadOnlyDictionary<string, FieldRegistry> registries)
    {
        return spec.Type switch
        {
            "PersonOrgExists" => CreatePersonOrgExists(spec, registries),
            "OrgManagerExists" => CreateOrgManagerExists(spec, registries),
            "OrgHierarchy" => CreateOrgHierarchy(spec, registries),

            _ => throw new InvalidOperationException(
                $"Unknown DataSetRule type '{spec.Type}'")
        };
    }

    // -------------------------------------------------------------

    private static IDataSetRule CreatePersonOrgExists(
        DataSetRuleSpec spec,
        IReadOnlyDictionary<string, FieldRegistry> registries)
    {
        var persName = Get(spec, "pers");
        var orgName = Get(spec, "org");

        var pers = registries[persName];
        var org = registries[orgName];

        return new PersonOrgExistsRule(
            persDataset: persName,
            orgDataset: orgName,
            persId: pers.Get(Get(spec, "persId")),
            orgRef: pers.Get(Get(spec, "orgRef")),
            orgId: org.Get(Get(spec, "orgId"))
        );
    }

    // -------------------------------------------------------------

    private static IDataSetRule CreateOrgManagerExists(
        DataSetRuleSpec spec,
        IReadOnlyDictionary<string, FieldRegistry> registries)
    {
        var orgName = Get(spec, "org");
        var persName = Get(spec, "pers");

        var org = registries[orgName];
        var pers = registries[persName];

        return new OrgManagerExistsRule(
            orgDataset: orgName,
            persDataset: persName,
            orgId: org.Get(Get(spec, "orgId")),
            manager: org.Get(Get(spec, "manager")),
            persId: pers.Get(Get(spec, "persId"))
        );
    }

    // -------------------------------------------------------------

    private static IDataSetRule CreateOrgHierarchy(
        DataSetRuleSpec spec,
        IReadOnlyDictionary<string, FieldRegistry> registries)
    {
        var orgName = Get(spec, "org");
        var org = registries[orgName];

        var rootValue = spec.Params.TryGetValue("rootValue", out var v)
            ? int.Parse(v)
            : 0;

        return new OrgHierarchyRule(
            orgDataset: orgName,
            id: org.Get(Get(spec, "id")),
            parent: org.Get(Get(spec, "parent")),
            rootValue: rootValue
        );
    }

    // -------------------------------------------------------------

    private static string Get(DataSetRuleSpec spec, string key)
    {
        if (!spec.Params.TryGetValue(key, out var value))
            throw new InvalidOperationException(
                $"Missing parameter '{key}' for rule '{spec.Type}'");

        return value;
    }
}
