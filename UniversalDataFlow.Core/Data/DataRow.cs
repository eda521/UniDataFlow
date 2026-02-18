namespace UniversalDataFlow.Core.Data;

using UniversalDataFlow.Core.Fields;

public sealed class DataRow
{
    private readonly Dictionary<FieldDefinition, object?> _values = new();

    public object? Get(FieldDefinition field)
        => _values.TryGetValue(field, out var value) ? value : null;

    public T? Get<T>(FieldDefinition field)
        => (T?)Get(field);

    public void Set(FieldDefinition field, object? value)
    {
        if (value != null && !field.DataType.IsAssignableFrom(value.GetType()))
            throw new InvalidOperationException(
                $"Invalid value type for field '{field.Name}'");

        _values[field] = value;
    }
}
