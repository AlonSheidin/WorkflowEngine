using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks.Implementation;

public class ValidateOrderTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        if (context.OrderId <= 0)
            return TaskResult.Failure;

        context.IsValid = true;
        return TaskResult.Success;
    }
}
