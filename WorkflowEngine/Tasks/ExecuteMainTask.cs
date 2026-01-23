using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks;

public class ExecuteMainTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        context.Counter++;
        return TaskResult.Success;
    }
}