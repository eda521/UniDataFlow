namespace UniversalDataFlow.Core.Fields;

public sealed class FieldDefinition
{
    public string Name { get; }
    public Type DataType { get; }

    public FieldDefinition(string name, Type dataType)
    {
        Name = name;
        DataType = dataType;
    }
}
