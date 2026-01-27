namespace WorkflowEngine.States;

public class ParallelState : State
{
    public TaskState[]? Branches { get; set; }
    public string? Next { get; set; } = null;
    public string? OnSuccess { get; set; } = null;
    public string? OnFailure { get; set; } = null;

    public ParallelState() : base(StateType.Parallel, "description")
    {
        if (Branches == null || Branches.Length == 0)
            throw new ArgumentException("Parallel state must have at least one branch");
        if (Next is null && (OnSuccess is null || OnFailure is null))
        {
            throw new ArgumentException("State must have a state to continue to");
        }

        if (Next is not null && (OnSuccess is not null || OnFailure is not null))
        {
            throw new ArgumentException("State cannot have next and OnSuccess or OnFailure states");
        }

        if (OnSuccess is not null && OnFailure is null || OnSuccess is null && OnFailure is not null)
        {
            throw new ArgumentException("A State with onSuccess must also have a OnFailure state, and the opposite");
        }
    }

    public override string ToString()
    {
        return base.ToString() + $"branches: [{Branches}],  next: {Next}, onSuccess: {OnSuccess}, onFailure: {OnFailure}";
    }
}