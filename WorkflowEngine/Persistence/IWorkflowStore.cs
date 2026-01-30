namespace WorkflowEngine.Persistence;

public interface IWorkflowStore
{
    public void SaveInstance(WorkflowInstance instance);
    public WorkflowInstance LoadInstance(int id);
    public void UpdateInstance(WorkflowInstance instance);
}