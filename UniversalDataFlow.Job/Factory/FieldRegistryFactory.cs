using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Job.Spec;

namespace UniversalDataFlow.Job.Factory;

public static class FieldRegistryFactory
{
    public static FieldRegistry Create(SourceSpec spec)
    {
        var registry = new FieldRegistry();

        foreach (var (name, type) in spec.Fields)
        {
            registry.Register(name, Type.GetType($"System.{type}", true)!);
        }

        return registry;
    }
}
