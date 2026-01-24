using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks.Implementation;

public class ChargePaymentTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        if (!context.IsValid)
            return TaskResult.Failure;

        context.PaymentCharged = true;
        return TaskResult.Success;
    }
}
