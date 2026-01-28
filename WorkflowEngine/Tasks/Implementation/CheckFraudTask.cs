using WorkflowEngine.Engine.Context;
using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Tasks.Implementation;

public class CheckFraudTask : ITask
{
    public async Task<(TaskResult, List<IContextEvent>)> ExecuteAsync(WorkflowContext context)
    {
        await Task.Delay(1000);
        Console.WriteLine($"Task {GetType().Name} running on thread {Thread.CurrentThread.ManagedThreadId}");
        bool fraudDetected = false; // simulate check
        return await Task.FromResult((fraudDetected ? TaskResult.Failure : TaskResult.Success, new List<IContextEvent>()));
    }
}
