namespace WorkflowEngine.States;

public class ParallelState() : State(StateType.Parallel, "description")
{
    public string[]? Branches { get; set; }
    public string? Next { get; set; } = null;
    public string? OnSuccess { get; set; } = null;
    public string? OnFailure { get; set; } = null;

    public override string ToString()
    {
        return base.ToString() + $"branches: [{string.Join(", ", Branches.Select(t => t.ToString()))}],  next: {Next}, onSuccess: {OnSuccess}, onFailure: {OnFailure}";
    }
}