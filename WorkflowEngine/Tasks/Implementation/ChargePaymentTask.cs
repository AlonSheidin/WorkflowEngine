using WorkflowEngine.Engine;
using WorkflowEngine.Engine.Context;
using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Tasks.Implementation;

public class ChargePaymentTask : ITask
{
    public Task<(TaskResult, List<IContextEvent>)> ExecuteAsync(
        WorkflowContext context)
    {
        if (!context.PaymentApproved)
        {
            return Task.FromResult(
                (TaskResult.Failure, new List<IContextEvent>()));
        }

        var events = new List<IContextEvent>
        {
            new SetEvent<string>(c => c.OrderStatus, "Paid")
        };

        return Task.FromResult((TaskResult.Success, events));
    }
}

