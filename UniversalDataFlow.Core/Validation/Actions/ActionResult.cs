namespace UniversalDataFlow.Core.Validation.Actions;

public sealed class ActionResult
{
    public ActionOutcome Outcome { get; }
    public string? Message { get; }

    private ActionResult(ActionOutcome outcome, string? message = null)
    {
        Outcome = outcome;
        Message = message;
    }

    public static ActionResult Continue()
        => new(ActionOutcome.Continue);

    public static ActionResult SkipRow(string reason)
        => new(ActionOutcome.SkipRow, reason);

    public static ActionResult StopPipeline(string reason)
        => new(ActionOutcome.StopPipeline, reason);
}
