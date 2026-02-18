namespace UniversalDataFlow.Core.Fields;

public sealed class FieldRegistry
{
    private readonly Dictionary<string, FieldDefinition> _fields = new();

    public FieldDefinition Register(string name, Type type)
    {
        if (_fields.ContainsKey(name))
            throw new InvalidOperationException($"Field '{name}' already exists.");

        var field = new FieldDefinition(name, type);
        _fields[name] = field;
        return field;
    }

    public FieldDefinition Get(string name)
        => _fields[name];

    public IEnumerable<FieldDefinition> All => _fields.Values;
}
