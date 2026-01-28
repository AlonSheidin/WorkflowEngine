namespace WorkflowEngine.Utility.Logger;

public class WorkflowEvent(
    WorkflowEventType eventType,
    string workflowName,
    string? stateName = null,
    string? taskName = null,
    string[]? parallelTaskNames = null)
{
    public WorkflowEventType EventType { get; } = eventType;
    public string WorkflowName { get; } = workflowName;
    public string? StateName { get; } = stateName;
    public string? TaskName { get; } = taskName;
    public string[]? ParallelTaskNames { get; } = parallelTaskNames;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}