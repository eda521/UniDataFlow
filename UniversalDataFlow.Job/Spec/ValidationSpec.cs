namespace UniversalDataFlow.Job.Spec;

/// <summary>
/// Declarative validation specification coming from JSON.
/// Used for both field and row validations.
/// </summary>
public sealed class ValidationSpec
{
    /// <summary>
    /// Rule identifier (e.g. NameRequired, AgeNonNegative)
    /// </summary>
    public string Rule { get; init; } = string.Empty;

    /// <summary>
    /// Optional action to apply when rule fails
    /// (e.g. SetZero, Normalize, SkipRow)
    /// </summary>
    public string Action { get; init; } = string.Empty;

    /// <summary>
    /// Validation policy: Continue | SkipRow | StopJob
    /// </summary>
    public string Policy { get; init; } = "StopJob";

    /// <summary>
    /// Rule parameters (field names, constants, etc.)
    /// </summary>
    public Dictionary<string, string> Params { get; init; } = new();
}
