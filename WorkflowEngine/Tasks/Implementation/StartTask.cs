using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks.Implementation;

public class StartTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        context.Initialized = true;
        context.OrderId = 1;
        context.Price = -10;
        return TaskResult.Success;
    }
}
