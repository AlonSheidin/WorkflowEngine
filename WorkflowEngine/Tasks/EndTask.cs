using WorkflowEngine.Engine;

namespace WorkflowEngine.Tasks;

public class EndTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        Console.WriteLine("Final Context:");
        Console.WriteLine(context);
        return TaskResult.Success;
    }
}