namespace UniversalDataFlow.Job.Spec;

public sealed class PipelineSpecDto
{
    public List<ValidationSpec> FieldValidations { get; init; } = new();
    public List<ValidationSpec> RowValidations { get; init; } = new();
}
