using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks.Implementation;

public class ShipOrderTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        if (!context.PaymentCharged)
            return TaskResult.Failure;

        context.Shipped = true;
        return TaskResult.Success;
    }
}
