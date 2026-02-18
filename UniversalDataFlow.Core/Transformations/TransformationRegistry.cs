namespace UniversalDataFlow.Core.Transformations;

public sealed class TransformationRegistry
{
    private readonly List<ITransformation> _transformations = new();

    public void Register(ITransformation transformation)
        => _transformations.Add(transformation);

    public IEnumerable<ITransformation> All => _transformations;
}
