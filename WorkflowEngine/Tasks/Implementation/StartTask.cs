using WorkflowEngine.Engine;
using WorkflowEngine.Engine.Context;
using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Tasks.Implementation;

public class StartTask : ITask
{
    public Task<(TaskResult, List<IContextEvent>)> ExecuteAsync(
        WorkflowContext context)
    {
        var events = new List<IContextEvent>
        {
            new SetEvent<string>(c => c.OrderStatus, "Started")
        };

        return Task.FromResult((TaskResult.Success, events));
    }
}

