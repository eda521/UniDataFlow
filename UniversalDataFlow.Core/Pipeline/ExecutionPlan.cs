namespace UniversalDataFlow.Core.Pipeline;

using UniversalDataFlow.Core.Transformations;

public sealed class ExecutionPlan
{
    public IReadOnlyList<ITransformation> Steps { get; }

    public ExecutionPlan(IReadOnlyList<ITransformation> steps)
    {
        Steps = steps;
    }
}
