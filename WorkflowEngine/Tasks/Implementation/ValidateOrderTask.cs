using WorkflowEngine.Engine;
using WorkflowEngine.Engine.Context;
using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Tasks.Implementation;

public class ValidateOrderTask : ITask
{
    public Task<(TaskResult, List<IContextEvent>)> ExecuteAsync(
        WorkflowContext context)
    {
        if (string.IsNullOrEmpty(context.OrderStatus))
        {
            return Task.FromResult(
                (TaskResult.Failure, new List<IContextEvent>()));
        }

        var events = new List<IContextEvent>
        {
            new SetEvent<bool>(c => c.PaymentApproved, true)
        };
        
        return Task.FromResult((TaskResult.Success, events));
    }
}

