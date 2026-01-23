using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks;

public class FailTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        return TaskResult.Success;
    }
}