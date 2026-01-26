using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks.Implementation;

public class ValidateOrderTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        Thread.Sleep(5000);
        
        if (context.OrderId <= 0)
            return TaskResult.Failure;

        context.IsValid = true;
        return TaskResult.Success;
    }

    public async Task<TaskResult> ExecuteAsync(WorkflowContext context)
    {
        await Task.Delay(5000);
        
        
        if (context.OrderId <= 0)
            return TaskResult.Failure;

        context.IsValid = true;
        return TaskResult.Success;
    }
}
