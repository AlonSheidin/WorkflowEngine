namespace WorkflowEngine.Utility.Logger;

public enum WorkflowEventType
{
    WorkflowStarted,
    WorkflowCompleted,
    StateEntered,
    StateExited,
    TaskStarted,
    TaskCompleted,
    TaskFailed,
    ParallelTasksStarted,
    ParallelTasksCompleted,
    ParallelTasksFailed,
}

