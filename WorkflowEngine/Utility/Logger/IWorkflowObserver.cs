namespace WorkflowEngine.Utility.Logger;

public interface IWorkflowObserver
{
    void OnWorkflowEvent(WorkflowEvent evt);
}
