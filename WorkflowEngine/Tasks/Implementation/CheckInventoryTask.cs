using WorkflowEngine.Engine.Context;
using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Tasks.Implementation;

public class CheckInventoryTask : ITask
{
    public async Task<(TaskResult, List<IContextEvent>)> ExecuteAsync(WorkflowContext context)
    {
        bool inStock = true; // simulate check
        var events = new List<IContextEvent>();
        
        await Task.Delay(10000);
        Console.WriteLine($"Task {GetType().Name} running on thread {Thread.CurrentThread.ManagedThreadId}");
        
        if (!inStock) return await Task.FromResult((TaskResult.Failure, events));
        
        return await Task.FromResult((TaskResult.Success, events));
    }
}
