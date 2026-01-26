using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks.Implementation;

public class ShipOrderTask : ITask
{
    public async Task<TaskResult> ExecuteAsync(WorkflowContext context)
    {
        if (!context.PaymentCharged)
            return TaskResult.Failure;
        
        await Task.Delay(100);
        
        context.Shipped = true;
        return TaskResult.Success;
    }

    public TaskResult Execute(WorkflowContext context)
    {
        if (!context.PaymentCharged)
            return TaskResult.Failure;
        
        Thread.Sleep(1000);
        
        context.Shipped = true;
        return TaskResult.Success;
    }
}
