namespace WorkflowEngine.Utility.Logger;

public class ConsoleLogger : IWorkflowObserver
{
    public void OnWorkflowEvent(WorkflowEvent evt)
    {
        switch (evt.EventType)
        {
            case WorkflowEventType.WorkflowStarted:
                Console.WriteLine($"[{evt.Timestamp}] Workflow '{evt.WorkflowName}' started.");
                break;
            case WorkflowEventType.WorkflowCompleted:
                Console.WriteLine($"[{evt.Timestamp}] Workflow '{evt.WorkflowName}' completed.");
                break;
            case WorkflowEventType.StateEntered:
                Console.WriteLine($"[{evt.Timestamp}] Entered state '{evt.StateName}' in workflow '{evt.WorkflowName}'.");
                break;
            case WorkflowEventType.StateExited:
                Console.WriteLine($"[{evt.Timestamp}] Exited state '{evt.StateName}' in workflow '{evt.WorkflowName}'.");
                break;
            case WorkflowEventType.TaskStarted:
                Console.WriteLine($"[{evt.Timestamp}] Task in state '{evt.StateName}' started.");
                break;
            case WorkflowEventType.TaskCompleted:
                Console.WriteLine($"[{evt.Timestamp}] Task in state '{evt.StateName}' completed successfully.");
                break;
            case WorkflowEventType.TaskFailed:
                Console.WriteLine($"[{evt.Timestamp}] Task in state '{evt.StateName}' failed.");
                break;
        }
    }
}
