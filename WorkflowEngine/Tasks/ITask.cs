using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks;

public interface ITask
{
    public TaskResult Execute(WorkflowContext context);
}