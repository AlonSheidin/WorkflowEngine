using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks;

public class StartTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        context.IsValid = true;
        context.Counter = 1;
        Console.WriteLine(" -> Start Task");
        return TaskResult.Success;
    }
}