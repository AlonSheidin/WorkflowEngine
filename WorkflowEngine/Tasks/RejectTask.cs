using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks;

public class RejectTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        context.Counter--;
        return TaskResult.Success;
    }
}