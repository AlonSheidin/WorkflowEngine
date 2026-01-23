using WorkflowEngine.Tasks;

namespace WorkflowEngine.Engine;

public static class StateHandlerRegistry
{
    private static readonly Dictionary<string, ITask> Tasks = new()
    {
        { "Start", new StartTask() },
        {"End", new EndTask() },
        {"Fail", new FailTask() },
        {"Reject", new RejectTask() },
        {"ExecuteMain", new ExecuteMainTask() }
    };
    
    
    public static ITask? GetTaskByName(string name)
    {
        return Tasks.GetValueOrDefault(name);
    }
}