using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks.Implementation;

public class ChargePaymentTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        if (!context.IsValid)
            return TaskResult.Failure;
        
        Thread.Sleep(5000);
        
        context.PaymentCharged = true;
        return TaskResult.Success;
    }

    public async Task<TaskResult> ExecuteAsync(WorkflowContext context)
    {
        if (!context.IsValid)
            return TaskResult.Failure;
        
        await Task.Delay(5000);
        
        context.PaymentCharged = true;
        return TaskResult.Success;
    }
}
