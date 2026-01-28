using WorkflowEngine.Engine.Context;
using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Tasks.Implementation;

public class CheckCustomerStatusTask : ITask
{
    public async Task<(TaskResult, List<IContextEvent>)> ExecuteAsync(WorkflowContext context)
    {
        await Task.Delay(1000);
        Console.WriteLine($"Task {GetType().Name} running on thread {Thread.CurrentThread.ManagedThreadId}");
        bool validCustomer = true; // simulate check
        return await Task.FromResult((validCustomer ? TaskResult.Success : TaskResult.Failure, new List<IContextEvent>()));
    }
}
